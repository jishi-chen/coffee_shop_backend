using coffee_shop_backend.Interface;
using coffee_shop_backend.Models;
using coffee_shop_backend.ViewModels;
using Dapper;
using System.Data;

namespace coffee_shop_backend.Repository
{
    public class DocumentRepository : GenericRepository, IDocumentRepository
    {
        public DocumentRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        public IEnumerable<Document> GetAdminList()
        {
            string sql = $@" select * from Documents ";
            return Connection.Query<Document>(sql, new { }, Transaction);
        }
        public DocumentInfoPage GetInfoPage(string documentId)
        {
            string sql = $@" select * from Documents where Id=@Id ";
            return Connection.QuerySingle<DocumentInfoPage>(sql, new { Id = documentId }, Transaction);
        }
        public IEnumerable<DocumentField> GetQuestionFieldList(string documentId)
        {
            string sql = $@" select * from DocumentFields where DocumentId=@DocumentId ";
            return Connection.Query<DocumentField>(sql, new { DocumentId = documentId }, Transaction);
        }
        public void InsertDocument(Document document)
        {
            string sql = $@"
insert into Documents (CsId, Caption, IsEnabled, Sort, StartDate, EndDate, HeadText, FooterText, Hits, Creator, CreateDate)
values (@CsId, @Caption, @IsEnabled, @Sort, @StartDate, @EndDate, @HeadText, @FooterText, @Hits, @Creator, @CreateDate ) ";
            Connection.Execute(sql, document, Transaction);
        }
        public void UpdateDocument(Document document)
        {
            string sql = $@"
update Documents set
CsId=@CsId, Caption=@Caption, IsEnabled=@IsEnabled, Sort=@Sort, StartDate=@StartDate, EndDate=@EndDate, HeadText=@HeadText, 
FooterText=@FooterText, Updator=@Updator, UpdateDate=@UpdateDate 
where Id=@Id ";
            Connection.Execute(sql, document, Transaction);
        }
        public int InsertDocumentField(DocumentField field)
        {
            string sql = $@"
insert into DocumentFields (ParentId, DocumentId, FieldName, Note, FieldType, WordLimit, RowLimit, FileSizeLimit, FileExtension, IsRequired, IsIncludedExport,
IsEditable, Sort, Creator, CreateDate)
values (@ParentId, @DocumentId, @FieldName, @Note, @FieldType, @WordLimit, @RowLimit, @FileSizeLimit, @FileExtension, @IsRequired, @IsIncludedExport,
@IsEditable, @Sort, @Creator, @CreateDate) 
select @@identity Id ";
            return Connection.QuerySingle<int>(sql, field, Transaction);
        }
        public void UpdateDocumentField(DocumentField field)
        {
            string sql = $@"
update DocumentFields set
ParentId=@ParentId, FieldName=@FieldName, Note=@Note, FieldType=@FieldType, WordLimit=@WordLimit, RowLimit=@RowLimit, FileSizeLimit=@FileSizeLimit, 
FileExtension=@FileExtension, IsRequired=@IsRequired, IsIncludedExport=@IsIncludedExport, IsEditable=@IsEditable, Sort=@Sort, Updator=@Updator, UpdateDate=@UpdateDate 
where Id=@Id ";
            Connection.Execute(sql, field, Transaction);
        }
        public void DeleteFieldOptions(string fieldId)
        {
            string sql = $@"";
            Connection.Execute(sql, new {}, Transaction);
        }
        public void InsertFieldOption(DocumentFieldOption option)
        {
            string sql = $@"";
            Connection.Execute(sql, option, Transaction);
        }
        public DocumentField GetDocumentField(string fieldId)
        {
            string sql = $@" select * from DocumentFields where Id=@Id ";
            return Connection.QuerySingle<DocumentField>(sql, new { Id = fieldId }, Transaction);
        }
        public IEnumerable<DocumentFieldOption> GetFieldOption(string fieldId)
        {
            string sql = $@" select * from DocumentFieldOptions where DocumentFieldId=@DocumentFieldId ";
            return Connection.Query<DocumentFieldOption>(sql, new { DocumentFieldId = fieldId }, Transaction);
        }
    }
}
