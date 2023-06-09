using coffee_shop_backend.Enums;
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

        public IEnumerable<Document> GetAdminList(bool? isEnabled)
        {
            string sql = $@" select * from Documents ";
            if (isEnabled.HasValue)
                sql += " where IsEnabled=@IsEnabled ";
            return Connection.Query<Document>(sql, new { IsEnabled = isEnabled }, Transaction);
        }
        public Document GetDocument(string documentId)
        {
            string sql = $@" select * from Documents where Id=@Id ";
            return Connection.QuerySingle<Document>(sql, new { Id = documentId }, Transaction);
        }
        public IEnumerable<DocumentField> GetFieldList(string documentId)
        {
            string sql = $@" select * from DocumentFields where DocumentId=@DocumentId order by Sort ";
            return Connection.Query<DocumentField>(sql, new { DocumentId = documentId }, Transaction);
        }
        public IEnumerable<DocumentField> GetFieldList(string documentId, string parentId)
        {
            string sql = $@" select * from DocumentFields where DocumentId=@DocumentId ";
            if (!string.IsNullOrEmpty(parentId))
                sql += " and ParentId=@ParentId ";
            else
                sql += " and ParentId is null ";
            sql += " order by Sort ";
            return Connection.Query<DocumentField>(sql, new { DocumentId = documentId, ParentId = parentId }, Transaction);
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
ParentId=@ParentId, FieldName=@FieldName, Note=@Note, FieldType=@FieldType, WordLimit=@WordLimit, RowLimit=@RowLimit, FileSizeLimit=@FileSizeLimit, Sort=@Sort, 
FileExtension=@FileExtension, IsRequired=@IsRequired, IsIncludedExport=@IsIncludedExport, IsEditable=@IsEditable, Updator=@Updator, UpdateDate=@UpdateDate 
where Id=@Id ";
            Connection.Execute(sql, field, Transaction);
        }
        public void DeleteFieldOptions(string fieldId)
        {
            string sql = $@" delete from DocumentFieldOptions where DocumentFieldId=@DocumentFieldId ";
            Connection.Execute(sql, new { DocumentFieldId  = fieldId }, Transaction);
        }
        public void InsertFieldOption(DocumentFieldOption option)
        {
            string sql = $@"
insert into DocumentFieldOptions (DocumentFieldId, OptionName, MemoType, Sort) 
values (@DocumentFieldId, @OptionName, @MemoType, @Sort) ";
            Connection.Execute(sql, option, Transaction);
        }
        public DocumentField GetDocumentField(string fieldId)
        {
            if (string.IsNullOrEmpty(fieldId))
                return new DocumentField();
            string sql = $@" select * from DocumentFields where Id=@Id ";
            return Connection.QuerySingle<DocumentField>(sql, new { Id = fieldId }, Transaction);
        }
        public DocumentField GetDocumentField(string documentId,string parentId, int sort)
        {
            string sql = $@" select top 1 * from DocumentFields where DocumentId=@DocumentId and Sort=@Sort ";
            if (!string.IsNullOrEmpty(parentId))
                sql += " and ParentId=@ParentId ";
            return Connection.QuerySingle<DocumentField>(sql, new { DocumentId = documentId, ParentId = parentId, Sort = sort }, Transaction);
        }
        public IEnumerable<DocumentFieldOption> GetFieldOption(string fieldId)
        {
            string sql = $@" select * from DocumentFieldOptions where DocumentFieldId=@DocumentFieldId ";
            return Connection.Query<DocumentFieldOption>(sql, new { DocumentFieldId = fieldId }, Transaction);
        }
        public DocumentFieldOption GetFieldOption(string optionId, string fieldId)
        {
            string sql = $@" select * from DocumentFieldOptions where Id=@Id ";
            return Connection.QuerySingle<DocumentFieldOption>(sql, new { Id = optionId }, Transaction);
        }
        public void DeleteField(string fieldId)
        {
            string sql = $@" delete from DocumentFields where Id=@Id ";
            Connection.Execute(sql, new { Id = fieldId }, Transaction);
        }
        public void UpdateFieldSort(IEnumerable<DocumentField> fields)
        {
            string sql = $@" update DocumentFields set Sort=@Sort where Id=@Id ";
            Connection.Execute(sql, fields, Transaction);
        }

        public DocumentRecord? GetDocumentRecord(string fieldId, string recordId)
        {
            string sql = $@" select * from DocumentRecords where DocumentFieldId=@DocumentFieldId and RegId=@RegId ";
            return Connection.QuerySingle<DocumentRecord>(sql, new { DocumentFieldId = fieldId, RegId = recordId }, Transaction);
        }
        public IEnumerable<DocumentRecord> GetDocumentRecord(string recordId)
        {
            string sql = $@" select * from DocumentRecords where RegId=@RegId ";
            return Connection.Query<DocumentRecord>(sql, new { RegId = recordId }, Transaction);
        }
        public IEnumerable<DocumentRecord> GetDocumentRecord()
        {
            string sql = $@" select * from DocumentRecords where 1=1 ";
            return Connection.Query<DocumentRecord>(sql, new { }, Transaction);
        }
        public IEnumerable<DocumentRecord> GetDocumentRecordList()
        {
            string sql = $@" select distinct RegId, DocumentId from DocumentRecords where 1=1 ";
            return Connection.Query<DocumentRecord>(sql, new { }, Transaction);
        }
        public IEnumerable<DocumentRecordViewModel> GetDocumentRecordData(string recordId, string documentId)
        {
            string sql = $@"
select df.ParentId, df.Id as FieldId, df.FieldName, df.FieldType, df.Sort, df.IsRequired, df.IsEditable, dr.FilledText, dr.MemoText as MemoValue, dr.Remark
from
(select *
from DocumentRecords
where RegId=@RegId) dr
right join DocumentFields df on dr.DocumentFieldId = df.Id
where df.DocumentId=@DocumentId
order by df.Sort asc, df.ParentId asc ";

            return Connection.Query<DocumentRecordViewModel>(sql, new { RegId = recordId, DocumentId = documentId }, Transaction);
        }
        public void InsertDocumentRecord(DocumentRecord record, string recordId)
        {
            string sql = $@"
insert into DocumentRecords (RegId, DocumentId, DocumentFieldId, FilledText, MemoText, Remark) 
values (@RegId, @DocumentId, @DocumentFieldId, @FilledText, @MemoText, @Remark) ";
            Connection.Execute(sql, record, Transaction);
        }
    }
}
