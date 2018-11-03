using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WIM.Core.Common.Utility.AppStates;
using WIM.Core.Common.Utility.Extensions;
using WIM.Core.Common.Utility.Extentions;
using WIM.Core.Common.Utility.UtilityHelpers;
using WIM.Core.Common.Utility.Validation;
using AppValidation = WIM.Core.Common.Utility.Validation;

namespace WIM.Core.Entity
{
    public static class BaseEntityExtensions
    {
        public static WriteDataState GetWriteDataState<TEntity>(this TEntity entity)
        {
            Type typeEntity = entity.GetType();
            PropertyInfo[] properties = typeEntity.GetProperties();
            List<string> namePropKeys = typeEntity.GetPropertiesName<KeyAttribute>(entity);

            if (!namePropKeys.Any())
            {
                throw new Exception("NamePropKeys Not Found!");
            }

            object id = typeEntity.GetProperty(namePropKeys[0]).GetValue(entity, null);
            Type type = typeEntity.GetProperty(namePropKeys[0]).PropertyType;
            if (id == null)
            {
                return WriteDataState.INSERT;
            }
            else if (type == typeof(Int32) && Convert.ToInt32(id) == 0)
            {
                return WriteDataState.INSERT;
            }
            else if (type == typeof(String) && Convert.ToString(id) == "")
            {
                return WriteDataState.INSERT;
            }
            else
            {
                return WriteDataState.UPDATE;
            }
        }

        public static bool IsInsertDataState<TEntity>(this TEntity entity)
        {
            return GetWriteDataState(entity) == WriteDataState.INSERT;
        }

        public static bool IsUpdateDataState<TEntity>(this TEntity entity)
        {
            return GetWriteDataState(entity) == WriteDataState.UPDATE;
        }

        public static void TrySetProjectIDSys<TEntity>(this TEntity entity)
        {
            try
            {
                IIdentity identity = UtilityHelper.GetIdentity();

                if (identity == null)
                {
                    return;
                }
                
                PropertyInfo propertyInfo = entity.GetType().GetProperty("ProjectIDSys");
                object _projectIDSys = propertyInfo.GetValue(entity, null);
                int projectIDSys = 0;
                if (!int.TryParse(_projectIDSys + "", out projectIDSys))
                {
                    projectIDSys = identity.GetProjectIDSys();
                }

                propertyInfo.SetValue(entity, Convert.ChangeType(projectIDSys, propertyInfo.PropertyType), null);
            }
            catch (Exception)
            {

            }
        }

        public static TEntity TryValidationNotNullException<TEntity>(this TEntity entity)
        {
            if (entity == null)
            {
                throw new AppValidation.AppValidationException(ErrorEnum.DATA_NOT_FOUND);
            }
            return entity;
        }

        public static List<TEntity> TryValidationNotNullException<TEntity>(this List<TEntity> entity)
        {
            if (entity == null || !entity.Any())
            {
                throw new AppValidation.AppValidationException(ErrorEnum.DATA_NOT_FOUND);
            }
            return entity;
        }

        public static IEnumerable<TEntity> TryValidationNotNullException<TEntity>(this IEnumerable<TEntity> entity)
        {
            if (entity == null || !entity.Any())
            {
                throw new AppValidation.AppValidationException(ErrorEnum.DATA_NOT_FOUND);
            }
            return entity;
        }
    }

}
