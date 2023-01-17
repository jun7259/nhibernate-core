﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using NHibernate.Id;
using NHibernate.Persister.Entity;
using NHibernate.Proxy;
using NHibernate.Type;

namespace NHibernate.Engine
{
	using System.Threading.Tasks;
	using System.Threading;
	public static partial class ForeignKeys
	{

		public partial class Nullifier
		{

			/// <summary> 
			/// Nullify all references to entities that have not yet 
			/// been inserted in the database, where the foreign key
			/// points toward that entity
			/// </summary>
			public async Task NullifyTransientReferencesAsync(object[] values, IType[] types, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				for (int i = 0; i < types.Length; i++)
				{
					values[i] = await (NullifyTransientReferencesAsync(values[i], types[i], cancellationToken)).ConfigureAwait(false);
				}
			}

			/// <summary> 
			/// Return null if the argument is an "unsaved" entity (ie. 
			/// one with no existing database row), or the input argument 
			/// otherwise. This is how Hibernate avoids foreign key constraint
			/// violations.
			/// </summary>
			private async Task<object> NullifyTransientReferencesAsync(object value, IType type, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				if (value == null)
				{
					return null;
				}
				else if (type.IsEntityType)
				{
					EntityType entityType = (EntityType)type;
					if (entityType.IsOneToOne)
					{
						return value;
					}
					else
					{
						string entityName = entityType.GetAssociatedEntityName();
						return await (IsNullifiableAsync(entityName, value, cancellationToken)).ConfigureAwait(false) ? null : value;
					}
				}
				else if (type.IsAnyType)
				{
					return await (IsNullifiableAsync(null, value, cancellationToken)).ConfigureAwait(false) ? null : value;
				}
				else if (type.IsComponentType)
				{
					IAbstractComponentType actype = (IAbstractComponentType)type;
					object[] subvalues = await (actype.GetPropertyValuesAsync(value, session, cancellationToken)).ConfigureAwait(false);
					IType[] subtypes = actype.Subtypes;
					bool substitute = false;
					for (int i = 0; i < subvalues.Length; i++)
					{
						object replacement = await (NullifyTransientReferencesAsync(subvalues[i], subtypes[i], cancellationToken)).ConfigureAwait(false);
						if (replacement != subvalues[i])
						{
							substitute = true;
							subvalues[i] = replacement;
						}
					}
					if (substitute)
						actype.SetPropertyValues(value, subvalues);
					return value;
				}
				else
				{
					return value;
				}
			}

			/// <summary> 
			/// Determine if the object already exists in the database, using a "best guess"
			/// </summary>
			private async Task<bool> IsNullifiableAsync(string entityName, object obj, CancellationToken cancellationToken)
			{
				cancellationToken.ThrowIfCancellationRequested();
				//if (obj == org.hibernate.intercept.LazyPropertyInitializer_Fields.UNFETCHED_PROPERTY)
				//  return false; //this is kinda the best we can do...

				if (obj.IsProxy())
				{
                    INHibernateProxy proxy = obj as INHibernateProxy;
                    
                    // if its an uninitialized proxy it can't be transient
					ILazyInitializer li = proxy.HibernateLazyInitializer;
					if (li.GetImplementation(session) == null)
					{
						return false;
						// ie. we never have to null out a reference to
						// an uninitialized proxy
					}
					else
					{
						//unwrap it
						obj = await (li.GetImplementationAsync(cancellationToken)).ConfigureAwait(false);
					}
				}

				// if it was a reference to self, don't need to nullify
				// unless we are using native id generation, in which
				// case we definitely need to nullify
				if (obj == self)
				{
					// TODO H3.2: Different behaviour
					//return isEarlyInsert || (isDelete && session.Factory.Dialect.HasSelfReferentialForeignKeyBug);
					return isEarlyInsert || isDelete;
				}

				// See if the entity is already bound to this session, if not look at the
				// entity identifier and assume that the entity is persistent if the
				// id is not "unsaved" (that is, we rely on foreign keys to keep
				// database integrity)
				EntityEntry entityEntry = session.PersistenceContext.GetEntry(obj);
				if (entityEntry == null)
				{
					return await (IsTransientSlowAsync(entityName, obj, session, cancellationToken)).ConfigureAwait(false);
				}
				else
				{
					return entityEntry.IsNullifiable(isEarlyInsert, session);
				}
			}
		}

		/// <summary> 
		/// Is this instance persistent or detached?
		/// </summary>
		/// <remarks>
		/// Hit the database to make the determination.
		/// </remarks>
		public static async Task<bool> IsNotTransientSlowAsync(string entityName, object entity, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return !await (IsTransientSlowAsync(entityName, entity, session, cancellationToken)).ConfigureAwait(false);
		}

		/// <summary> 
		/// Is this instance, which we know is not persistent, actually transient? 
		/// Don't hit the database to make the determination, instead return null; 
		/// </summary>
		/// <remarks>
		/// Don't hit the database to make the determination, instead return null; 
		/// </remarks>
		public static async Task<bool?> IsTransientFastAsync(string entityName, object entity, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (Equals(Intercept.LazyPropertyInitializer.UnfetchedProperty, entity))
			{
				// an unfetched association can only point to
				// an entity that already exists in the db
				return false;
			}

			var proxy = entity as INHibernateProxy;
			if (proxy?.HibernateLazyInitializer.IsUninitialized == true)
			{
				return false;
			}

			// let the interceptor inspect the instance to decide
			var interceptorResult = session.Interceptor.IsTransient(entity);
			if (interceptorResult.HasValue)
				return interceptorResult;

			// let the persister inspect the instance to decide
			if (proxy != null)
			{
				// The persister only deals with unproxied entities.
				entity = await (proxy.HibernateLazyInitializer.GetImplementationAsync(cancellationToken)).ConfigureAwait(false);
			}

			return await (session
				.GetEntityPersister(
					entityName,
					entity)
				.IsTransientAsync(entity, session, cancellationToken)).ConfigureAwait(false);
		}

		/// <summary> 
		/// Is this instance, which we know is not persistent, actually transient? 
		/// </summary>
		/// <remarks>
		/// Hit the database to make the determination.
		/// </remarks>
		public static async Task<bool> IsTransientSlowAsync(string entityName, object entity, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await (IsTransientFastAsync(entityName, entity, session, cancellationToken)).ConfigureAwait(false) ??
			       await (HasDbSnapshotAsync(entityName, entity, session, cancellationToken)).ConfigureAwait(false);
		}

		static async Task<bool> HasDbSnapshotAsync(string entityName, object entity, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			IEntityPersister persister = session.GetEntityPersister(entityName, entity);
			if (persister.IdentifierGenerator is Assigned)
			{
				// When using assigned identifiers we cannot tell if an entity
				// is transient or detached without querying the database.
				// This could potentially cause Select N+1 in cascaded saves, so warn the user.
				log.Warn("Unable to determine if {0} with assigned identifier {1} is transient or detached; querying the database. Use explicit Save() or Update() in session to prevent this.", 
					entity, persister.GetIdentifier(entity));
			}

			// hit the database, after checking the session cache for a snapshot
			System.Object[] snapshot =
				await (session.PersistenceContext.GetDatabaseSnapshotAsync(persister.GetIdentifier(entity), persister, cancellationToken)).ConfigureAwait(false);
			return snapshot == null;
		}

		/// <summary> 
		/// Return the identifier of the persistent or transient object, or throw
		/// an exception if the instance is "unsaved"
		/// </summary>
		/// <remarks>
		/// Used by OneToOneType and ManyToOneType to determine what id value should 
		/// be used for an object that may or may not be associated with the session. 
		/// This does a "best guess" using any/all info available to use (not just the 
		/// EntityEntry).
		/// </remarks>
		public static async Task<object> GetEntityIdentifierIfNotUnsavedAsync(string entityName, object entity, ISessionImplementor session, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			if (entity == null)
			{
				return null;
			}
			else
			{
				object id = session.GetContextEntityIdentifier(entity);
				if (id == null)
				{
					// context-entity-identifier returns null explicitly if the entity
					// is not associated with the persistence context; so make some deeper checks...

					/***********************************************/
					// NH-479 (very dirty patch)
					if (entity.GetType().IsPrimitive)
						return entity;
					/**********************************************/

					if ((await (IsTransientFastAsync(entityName, entity, session, cancellationToken)).ConfigureAwait(false)).GetValueOrDefault())
					{
						entityName = entityName ?? session.GuessEntityName(entity);
						string entityString = entity.ToString();
						throw new TransientObjectException(
							string.Format("object references an unsaved transient instance - save the transient instance before flushing or set cascade action for the property to something that would make it autosave. Type: {0}, Entity: {1}", entityName, entityString));
					}
					id = session.GetEntityPersister(entityName, entity).GetIdentifier(entity);
				}
				return id;
			}
		}
	}
}