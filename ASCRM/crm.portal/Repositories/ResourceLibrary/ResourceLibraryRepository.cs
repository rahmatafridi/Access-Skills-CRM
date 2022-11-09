using crm.portal.CrmModel;
using crm.portal.Interfaces.ResourceLibrary;
using crm.portal.OscaModel;
using Dapper;
using ds.core.uow;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace crm.portal.Repositories.ResourceLibrary
{
    public class ResourceLibraryRepository : IResourceLibraryRepository
    {
        private readonly IUOW_Osca _unitOfWork_OSCA;
        private readonly string _connString_OSCA;
        private readonly string _connString_Portal;
        private readonly string _connString;
        private readonly IUOW_Portal _unitOfWork_Portal;

        public ResourceLibraryRepository(IUOW unitOfWork, IUOW_Osca unitOfWork_OSCA, IUOW_Portal unitOfWork_Portal)
        {
            _connString_OSCA = unitOfWork_OSCA.GetConnectionString();
            _connString_Portal = unitOfWork_Portal.GetConnectionString();
            _connString = unitOfWork.GetConnectionString();

        }

        public bool AddDocCategory(PortalDocCategory docCategory)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@DocCat_Title", docCategory.DocCat_Title);
                    dynamicParams.Add("@DocCat_Access", docCategory.DocCat_Access);

                    if (docCategory.DocCat_Id > 0)
                    {
                        dynamicParams.Add("@DocCat_Id", docCategory.DocCat_Id);
                        string storedProc = "[dbo].[SP_PORTAL_UPDATE_DOC_CAT]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_PORTAL_ADD_DOC_CAT]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool AddDocResource(PortalDoc doc)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var dynamicParams = new DynamicParameters();
                    dynamicParams.Add("@Docs_Id_DocCategories", doc.Docs_Id_DocCategories);
                    dynamicParams.Add("@Docs_Title", doc.Docs_Title);
                    dynamicParams.Add("@Docs_IsActive", 1);
                    dynamicParams.Add("@Docs_EnteredBy", doc.Docs_EnteredBy);
                    dynamicParams.Add("@Docs_EnteredByUser", doc.Docs_EnteredByUser);
                    dynamicParams.Add("@Docs_FileDate", doc.Docs_FileDate);
                    dynamicParams.Add("@Docs_Version", doc.Docs_Version);
                    dynamicParams.Add("@Docs_File", doc.Docs_File);

                    if (doc.Docs_Id > 0)
                    {
                        dynamicParams.Add("@Docs_Id", doc.Docs_Id);
                        string storedProc = "[dbo].[SP_PORTAL_UPDATE_DOC_CAT]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                    else
                    {
                        string storedProc = "[dbo].[SP_PORTAL_ADD_RESOURCE]";
                        conn.Query(storedProc, param: dynamicParams, commandType: CommandType.StoredProcedure);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            return true;
        }

        public bool DeleteCate(int id)
        {

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PORTAL_DELETE_CAT]";
                    object dynamicParams = new
                    {
                        id = id
                    };

                    var data = conn.Query<bool>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return true;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public bool DeleteCateFile(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PORTAL_DELETE_CAT_FILE]";
                    object dynamicParams = new
                    {
                        id = id
                    };

                    var data = conn.Query<bool>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return true;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }

        }

        public PortalDocCategory GetDocCategoryById(int id)
        {
            PortalDocCategory courseConent = new PortalDocCategory ();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PROTAL_GET_CATBYID]";
                    object dynamicParams = new
                    {
                        Id= id
                    };

                    var data = conn.QueryFirstOrDefault<PortalDocCategory>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public PortalDoc GetRecourseFileById(int id)
        {
            PortalDoc courseConent = new PortalDoc();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PROTAL_GET_RES_FILEBYId]";
                    object dynamicParams = new
                    {
                        id = id
                    };

                    var data = conn.QueryFirstOrDefault<PortalDoc>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<DocCategory> LoadCategories(string code)
        {
            List<DocCategory> courseConent = new List<DocCategory>();
            List<Document> documents = new List<Document>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    var prame = 0;
                    if (code == "900")
                    {
                        prame = 2;
                    }
                    if (code == "1000")
                    {
                        prame = 1;
                    }

                    string storedProc = "[dbo].[SP_PORTAL_GET_DOC_CATEGORY]";
                    object dynamicParams = new
                    {
                      accesstype=prame
                    };

                    var data = conn.Query<DocCategory>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                   

                    foreach (var item in data)
                    {
                        string storedProc1 = "[dbo].[SP_PORTAL_GET_DOC_BY_CAT]";
                        object dynamicParams1 = new
                        {
                            CatId =item.DocCat_Id
                        };
                        documents = (List<Document>)conn.Query<Document>(storedProc1, param: dynamicParams1, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                        courseConent.Add(new DocCategory()
                        {
                            DocCat_Title = item.DocCat_Title,
                            DocCat_Id = item.DocCat_Id,
                            documents = documents
                        });
                    }
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<DocCategory> LoadCategoriesForAdmin()
        {
            List<DocCategory> courseConent = new List<DocCategory>();
            List<Document> documents = new List<Document>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    

                    string storedProc = "[dbo].[SP_PORTAL_GET_DOC_CATEGORY_FORADMIN]";
                    object dynamicParams = new
                    {
                       
                    };

                    var data = conn.Query<DocCategory>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    foreach (var item in data)
                    {
                        string storedProc1 = "[dbo].[SP_PORTAL_GET_DOC_BY_CAT]";
                        object dynamicParams1 = new
                        {
                            CatId = item.DocCat_Id
                        };
                        documents = (List<Document>)conn.Query<Document>(storedProc1, param: dynamicParams1, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                        courseConent.Add(new DocCategory()
                        {
                            DocCat_Title = item.DocCat_Title,
                            DocCat_Id = item.DocCat_Id,
                            documents = documents
                        });
                    }
                    return courseConent;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<PortalDocCategory> LoadDocCategory()
        {
            List<PortalDocCategory> courseConent = new List<PortalDocCategory>();
            List<PortalDocCategory> documents = new List<PortalDocCategory>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PORTAL_GET_DOC_CATEGORY_FORADMIN]";
                    object dynamicParams = new
                    {

                    };

                    var data = conn.Query<PortalDocCategory>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                    foreach (var item in data)
                    {
                        if(item.DocCat_Access == 2)
                        {
                            item.Type = "Learner";
                            
                        }
                        if (item.DocCat_Access == 1)
                        {
                            item.Type = "Assessor";

                        }
                        if (item.DocCat_Access == 3)
                        {
                            item.Type = "Public";

                        }
                        documents.Add(new PortalDocCategory()
                        {
                            DocCat_Access = item.DocCat_Access,
                            DocCat_Id = item.DocCat_Id,
                            DocCat_IsActive =item.DocCat_IsActive,
                            DocCat_Title = item.DocCat_Title,
                            DocCat_WeightOrder = item.DocCat_WeightOrder,
                            Type=item.Type
                        });
                    }
                    
                    return documents;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<PortalDoc> LoadDocFiles()
        {
            List<PortalDoc> courseConent = new List<PortalDoc>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PORTAL_GET_DOC_FILES]";
                    object dynamicParams = new
                    {

                    };

                    var data = conn.Query<PortalDoc>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public IEnumerable<PortalDoc> LoadDocFilesById(int id)
        {
            List<PortalDoc> courseConent = new List<PortalDoc>();
            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {


                    string storedProc = "[dbo].[SP_PORTAL_GET_DOC_FILES_BYCATEID]";
                    object dynamicParams = new
                    {
                        id=id
                    };

                    var data = conn.Query<PortalDoc>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);


                    return data;
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
        }

        public PrimeReviewStats LoadQuickStat(int id)
        {
            PrimeReviewStats courseConent = new PrimeReviewStats();
            using (SqlConnection conn = new SqlConnection(_connString_OSCA))
            {
                try
                {
                    string storedProc = "[dbo].[SP_SS_CPDGetPrimeReviewStats]";
                    object dynamicParams = new
                    {
                        cpdprime_learner_id = id
                    };
                    courseConent = conn.QueryFirstOrDefault<PrimeReviewStats>(storedProc, param: dynamicParams, commandTimeout: 60, commandType: CommandType.StoredProcedure);
                    return courseConent;
                }
                catch (Exception ex)
                {
                    throw;
                }

            }
        }

    }
}
