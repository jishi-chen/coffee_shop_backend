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
        public IEnumerable<DocumentRecord> GetDocumentRecordList(string? documentId)
        {
            string sql = $@" select distinct RegId, DocumentId from DocumentRecords where 1=1 ";
            if (!string.IsNullOrEmpty(documentId))
                sql += " and DocumentId=@DocumentId ";
            return Connection.Query<DocumentRecord>(sql, new { DocumentId = documentId }, Transaction);
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
        public void InsertDocumentRecord(IEnumerable<DocumentRecord> recordList)
        {
            string sql = $@"
insert into DocumentRecords (RegId, DocumentId, DocumentFieldId, FilledText, MemoText, Remark) 
values (@RegId, @DocumentId, @DocumentFieldId, @FilledText, @MemoText, @Remark) ";
            Connection.Execute(sql, recordList, Transaction);
        }
        public void UpdateDocumentRecord(IEnumerable<DocumentRecord> recordList)
        {
            string sql = $@"
update DocumentRecords set
FilledText=@FilledText, MemoText=@MemoText, Remark=@Remark
where RegId=@RegId and DocumentId=@DocumentId and DocumentFieldId=@DocumentFieldId ";
            Connection.Execute(sql, recordList, Transaction);
        }
        public DataTable GetExportData(string documentId)
        {
            string sql = $@"
DECLARE 
  @cols AS NVARCHAR(MAX),
  @query AS NVARCHAR(MAX),
  @ParmDefinition NVARCHAR(500),
  @SubQuery AS NVARCHAR(MAX);

set @ParmDefinition = '@id int, @SingleChoiceType int, @DoubleChoiceType int';

select @cols = STUFF((SELECT ',' + QUOTENAME(FieldName) 
                    from DocumentFields
                    where DocumentId = @DocumentId and FieldType != @PanelType and FieldType != @FileType
            FOR XML PATH(''), TYPE
            ).value('.', 'NVARCHAR(MAX)') 
        ,1,1,'');
set @SubQuery = N'
select record.RegId, field.FieldName, record.FilledText, field.FieldType,
STUFF((
select '';'' +cast(o.OptionName AS NVARCHAR )
from DocumentRecords r
join DocumentFields f on r.DocumentFieldId = f.Id and (f.FieldType = @SingleChoiceType or f.FieldType = @DoubleChoiceType)
join DocumentFieldOptions o on r.DocumentFieldId = o.DocumentFieldId and r.FilledText like ''%''+convert(nvarchar,o.Id)+''%''
where record.DocumentFieldId = r.DocumentFieldId
FOR XML PATH('''')), 1, 1, '''') as OptionText
from DocumentRecords record 
join DocumentFields field on record.DocumentFieldId = field.Id
where record.DocumentId = @id
';
set @query = N'
select * from (
select RegId, FieldName, CAST(CASE WHEN FieldType = @SingleChoiceType or FieldType = @DoubleChoiceType THEN OptionText ELSE FilledText END AS nvarchar) as FilledText
from (' + @SubQuery  + ') as y where 1=1
) t
pivot(
	MAX(FilledText) 
	FOR FieldName IN ( ' + @cols + ' )) p ';
	
exec sp_executesql @query, @ParmDefinition,@id = @DocumentId,@SingleChoiceType = @SingleChoiceType,@DoubleChoiceType = @DoubleChoiceType; ";

            object param = new
            {
                DocumentId = documentId,
                PanelType = AnswerTypeEnum.Panel,
                SingleChoiceType = AnswerTypeEnum.SingleChoice,
                DoubleChoiceType = AnswerTypeEnum.MultipleChoice,
                FileType = AnswerTypeEnum.File
            };
            var dt = new DataTable();
            var dr = Connection.ExecuteReader(sql, param, Transaction);
            dt.Load(dr);
            return dt;
        }
    }
}
