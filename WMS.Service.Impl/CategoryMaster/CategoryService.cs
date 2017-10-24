using AutoMapper;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WMS.Repository;
using WIM.Core.Common.Validation;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using WIM.Core.Common.Helpers;
using WMS.Common;
using WMS.Context;
using WMS.Entity.ItemManagement;
using WMS.Repository.Impl;

namespace WMS.Service
{
    public class CategoryService : ICategoryService
    {
        private CategoryRepository repo;
        private WMSDbContext proc;
        public CategoryService()
        {
            proc = new WMSDbContext();
            repo = new CategoryRepository();
        }

        public IEnumerable<CategoryDto> GetCategories()
        {
            IEnumerable<Category_MT> categorys = repo.Get();

            IEnumerable<CategoryDto> categoryDtos = categorys.Select(b => new CategoryDto()
            {
                CateIDSys = b.CateIDSys,
                CateID = b.CateID,
                CateName = b.CateName,
                Active = b.Active,
                ProjectIDSys = b.ProjectIDSys
            });
            return categoryDtos;
        }

        public IEnumerable<CategoryDto> GetCategoriesByProjectID(int projectIDSys)
        {
            IEnumerable<Category_MT> categorys = repo.GetByProjectID(projectIDSys);
            IEnumerable<CategoryDto> categoryDtos = categorys.Select(b => new CategoryDto()
            {
                CateIDSys = b.CateIDSys,
                CateID = b.CateID,
                CateName = b.CateName,
                Active = b.Active,
                ProjectIDSys = b.ProjectIDSys
            });
            return categoryDtos;
        }

        public CategoryDto GetCategory(int id)
        {
            Category_MT category = repo.GetByID(id);
            CategoryDto categoryDto = new CategoryDto();
            categoryDto.CateIDSys = category.CateIDSys;
            categoryDto.CateID = category.CateID;
            categoryDto.CateName = category.CateName;
            categoryDto.Active = category.Active;
            categoryDto.ProjectIDSys = category.ProjectIDSys;
            return categoryDto;
        }

        public int CreateCategory(Category_MT category)
        {
            using (var scope = new TransactionScope())
            {
                category.CateID = proc.ProcGetNewID("CT").FirstOrDefault();
                category.Active = 1;
                category.CreatedDate = DateTime.Now;
                category.UpdateDate = DateTime.Now;
                category.UserUpdate = "1";
                try
                {
                    repo.Insert(category);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                return category.CateIDSys;
            }
        }

        public bool UpdateCategory(int id, Category_MT category)
        {
            using (var scope = new TransactionScope())
            {
                var existedCategory = repo.GetByID(id);
                existedCategory.CateName = category.CateName;
                existedCategory.UpdateDate = DateTime.Now;
                existedCategory.UserUpdate = "1";
                try
                {
                    repo.Update(existedCategory);
                    scope.Complete();
                }
                catch (DbEntityValidationException e)
                {
                    HandleValidationException(e);
                }
                catch (DbUpdateException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4012));
                    throw ex;
                }
                
                return true;
            }
        }

        public bool DeleteCategory(int id)
        {
            using (var scope = new TransactionScope())
            {
                var existedCategory = repo.GetByID(id);
                existedCategory.Active = 0;
                existedCategory.UpdateDate = DateTime.Now;
                existedCategory.UserUpdate = "1";
                try
                {
                    repo.Update(existedCategory);
                    scope.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    scope.Dispose();
                    ValidationException ex = new ValidationException(Helper.GetHandleErrorMessageException(ErrorCode.E4017));
                    throw ex;
                }


                return true;
            }
        }

        public void HandleValidationException(DbEntityValidationException ex)
        {
            foreach (var eve in ex.EntityValidationErrors)
            {
                foreach (var ve in eve.ValidationErrors)
                {
                    throw new ValidationException(ve.PropertyName, ve.ErrorMessage);
                }
            }
        }

    }
}
