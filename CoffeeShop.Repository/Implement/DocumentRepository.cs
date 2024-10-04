using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using CoffeeShop.Repository.Interface;
using System.Data;
using Dapper;
using CoffeeShop.Model.Enum;
using CoffeeShop.Model;

namespace CoffeeShop.Repository.Implement
{
    public class DocumentRepository : GenericRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(IDbTransaction transaction, CoffeeShopContext context) : base(transaction, context) { }

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
insert into Documents (Caption, IsEnabled, Sort, StartDate, EndDate, HeadText, FooterText, Hits, Creator, CreateDate)
values (@Caption, @IsEnabled, @Sort, @StartDate, @EndDate, @HeadText, @FooterText, @Hits, @Creator, @CreateDate ) ";
            Connection.Execute(sql, document, Transaction);
        }
        public void UpdateDocument(Document document)
        {
            string sql = $@"
update Documents set
Caption=@Caption, IsEnabled=@IsEnabled, Sort=@Sort, StartDate=@StartDate, EndDate=@EndDate, HeadText=@HeadText, 
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
            Connection.Execute(sql, new { DocumentFieldId = fieldId }, Transaction);
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
        public DocumentField GetDocumentField(string documentId, string parentId, int sort)
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

        public DocumentRecordDetail? GetDocumentRecord(string fieldId, string recordId)
        {
            string sql = $@" select * from DocumentRecordDetails where DocumentFieldId=@DocumentFieldId and DocumentRecordId=@DocumentRecordId ";
            return Connection.QuerySingle<DocumentRecordDetail>(sql, new { DocumentFieldId = fieldId, DocumentRecordId = recordId }, Transaction);
        }
        public IEnumerable<DocumentRecordDetail> GetDocumentRecord(string recordId)
        {
            string sql = $@" select * from DocumentRecordDetails where DocumentRecordId=@DocumentRecordId ";
            return Connection.Query<DocumentRecordDetail>(sql, new { DocumentRecordId = recordId }, Transaction);
        }
        public IEnumerable<DocumentRecordDetail> GetDocumentRecord()
        {
            string sql = $@" select * from DocumentRecordDetails where 1=1 ";
            return Connection.Query<DocumentRecordDetail>(sql, new { }, Transaction);
        }
        public IEnumerable<DocumentRecordListViewModel> GetDocumentRecordList(string? documentId)
        {
            string sql = $@"  select distinct record.Id as RegId, record.DocumentId, d.Caption as DocumentName, record.Name as RegName
                             from DocumentRecords record
                             left join Documents d on record.DocumentId = d.Id
                             left join DocumentRecordDetails r on record.Id = r.DocumentRecordId
                             where 1=1 ";
            if (!string.IsNullOrEmpty(documentId))
                sql += " and DocumentId=@DocumentId ";
            return Connection.Query<DocumentRecordListViewModel>(sql, new { DocumentId = documentId }, Transaction);
        }
        public IEnumerable<DocumentRecordViewModel> GetDocumentRecordData(string recordId, string documentId)
        {
            string sql = $@"
select df.ParentId, df.Id as FieldId, df.FieldName, df.FieldType, df.Sort, df.IsRequired, df.IsEditable, dr.FilledText, dr.MemoText as MemoValue, dr.Remark
from
(select *
from DocumentRecordDetails
where DocumentRecordId=@DocumentRecordId) dr
right join DocumentFields df on dr.DocumentFieldId = df.Id
where df.DocumentId=@DocumentId
order by df.Sort asc, df.ParentId asc ";

            return Connection.Query<DocumentRecordViewModel>(sql, new { DocumentRecordId = recordId, DocumentId = documentId }, Transaction);
        }
        public int InsertDocumentRecord(DocumentRecord record)
        {
            string sql = $@"
insert into DocumentRecords (DocumentId, Name) 
values (@DocumentId, @Name)
SELECT SCOPE_IDENTITY(); ";
            return Connection.Query<int>(sql, record, Transaction).Single();
        }
        public void InsertDocumentRecordDetail(IEnumerable<DocumentRecordDetail> recordList)
        {
            string sql = $@"
insert into DocumentRecordDetails (DocumentRecordId, DocumentFieldId, FilledText, MemoText, Remark) 
values (@DocumentRecordId, @DocumentFieldId, @FilledText, @MemoText, @Remark) ";
            Connection.Execute(sql, recordList, Transaction);
        }
        public void UpdateDocumentRecordDetail(IEnumerable<DocumentRecordDetail> recordList)
        {
            string sql = $@"
update DocumentRecordDetails set
FilledText=@FilledText, MemoText=@MemoText, Remark=@Remark
where DocumentRecordId=@DocumentRecordId and DocumentFieldId=@DocumentFieldId ";
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
select record.DocumentRecordId, field.FieldName, record.FilledText, field.FieldType,
STUFF((
select '';'' +cast(o.OptionName AS NVARCHAR )
from DocumentRecordDetails r
join DocumentFields f on r.DocumentFieldId = f.Id and (f.FieldType = @SingleChoiceType or f.FieldType = @DoubleChoiceType)
join DocumentFieldOptions o on r.DocumentFieldId = o.DocumentFieldId and r.FilledText like ''%''+convert(nvarchar,o.Id)+''%''
join DocumentRecords drecord on r.DocumentRecordId = drecord.Id
where drecord.DocumentId = @id and record.DocumentFieldId = o.DocumentFieldId and r.DocumentRecordId = record.DocumentRecordId
FOR XML PATH('''')), 1, 1, '''') as OptionText
from DocumentRecordDetails record 
join DocumentRecords ddrecord on record.DocumentRecordId = ddrecord.Id
join DocumentFields field on record.DocumentFieldId = field.Id
where ddrecord.DocumentId = @id
';
set @query = N'
select * from (
select DocumentRecordId, FieldName, CAST(CASE WHEN FieldType = @SingleChoiceType or FieldType = @DoubleChoiceType THEN OptionText ELSE FilledText END AS nvarchar) as FilledText
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
