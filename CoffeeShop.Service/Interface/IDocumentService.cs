using CoffeeShop.Model.Entities;
using CoffeeShop.Model.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace CoffeeShop.Service.Interface
{
    public interface IDocumentService
    {
        void FieldDataCheck(IFormCollection collection, IEnumerable<DocumentFieldViewModel> fieldList, Dictionary<string, string> validResultList, string recordId);
        void Create(DocumentFormViewModel model, string recordId, IFormFileCollection fileCollection, bool isEdit);
        List<DocumentRecordViewModel> GetRecordData(string regId, string documentId);
        DocumentViewModel GetFormData(string id);
        IEnumerable<Document> GetAdminList();
        void InsertDocument(DocumentDTO model);
        void UpdateDocument(DocumentDTO model);
        List<DocumentRecordListViewModel> GetRecodList();
        void InsertField (DocumentViewModel viewModel, DocumentQuestionPage questionPage);
        void EditField(DocumentViewModel model, string fieldId);
        int DeleteField (string id);
        void SetFieldSort(DocumentViewModel model, string fieldId, bool direction);
        DocumentFormViewModel GetFrontFormData(string id, string recordId);
    }
}
