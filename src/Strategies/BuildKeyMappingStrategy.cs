using System;
using System.Linq;
using System.Reflection;
using Unity.Builder;
using Unity.Builder.Strategy;
using Unity.Lifetime;
using Unity.Policy;
using Unity.Policy.Mapping;
using Unity.Registration;
using Unity.Strategy;

namespace Unity.Strategies
{
    /// <summary>
    /// Represents a strategy for mapping build keys in the build up operation.
    /// </summary>
    public class BuildKeyMappingStrategy : BuilderStrategy, IRegisterTypeStrategy
    {
        #region BuilderStrategy

        /// <summary>
        /// Called during the chain of responsibility for a build operation.  Looks for the <see cref="IBuildKeyMappingPolicy"/>
        /// and if found maps the build key for the current operation.
        /// </summary>
        /// <param name="context">The context for the operation.</param>
        public override object PreBuildUp(IBuilderContext context)
        {
            if (context.OriginalBuildKey is TypeRegistration registration && 
                registration.RegisteredType == registration.MappedToType)
                return null;
                
            IBuildKeyMappingPolicy policy = context.PersistentPolicies.Get<IBuildKeyMappingPolicy>(context.OriginalBuildKey.Type, 
                                                                                                   context.OriginalBuildKey.Name, out _) 
                                          ?? (context.OriginalBuildKey.Type.GetTypeInfo().IsGenericType 
                                          ? context.Policies.Get<IBuildKeyMappingPolicy>(context.OriginalBuildKey.Type.GetGenericTypeDefinition(), 
                                                                                         context.OriginalBuildKey.Name, out _) 
                                          : null);

            if (null == policy) return null;

            context.BuildKey = policy.Map(context.BuildKey, context);

            if (!policy.RequireBuild && context.Container.IsRegistered(context.BuildKey.Type, context.BuildKey.Name))
            {
                context.Existing = context.NewBuildUp(context.BuildKey.Type, context.BuildKey.Name);
                context.BuildComplete = null != context.Existing;
            }

            return null;
        }

        #endregion


        #region IRegisterTypeStrategy

        public void RegisterType(IContainerContext context, Type typeFrom, Type typeTo, string name, 
                                 LifetimeManager lifetimeManager, params InjectionMember[] injectionMembers)
        {
            // Validate imput
            if (typeFrom == null || typeFrom == typeTo) return;

            // Require Re-Resolve if no injectors specified
            var buildRequired = lifetimeManager is IRequireBuildUpPolicy || 
                (null == injectionMembers ? false : injectionMembers.Any(m => m.BuildRequired));

            // Set mapping policy
            var policy = typeFrom.GetTypeInfo().IsGenericTypeDefinition && typeTo.GetTypeInfo().IsGenericTypeDefinition
                       ? new GenericTypeBuildKeyMappingPolicy(typeTo, name, buildRequired)
                       : (IBuildKeyMappingPolicy)new BuildKeyMappingPolicy(typeTo, name, buildRequired);
            context.Policies.Set(typeFrom, name, typeof(IBuildKeyMappingPolicy), policy);
        }

        #endregion

    }
}
