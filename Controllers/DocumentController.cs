using docvault_frontend.Data;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using docvault_frontend.Models;
using Newtonsoft.Json;

namespace docvault_frontend.Controllers;

public class DocumentController : Controller
{
    private readonly BackendCommunicator backend = new BackendCommunicator();

    public DocumentController()
    {

    }
    // GET: /Document/
    public async Task<IActionResult> Index() // Returns a table of Documents
    {
        return View(await backend.GetDocuments());
    }

    // POST: /Document/Download/{FileName}
    [HttpPost]
    public async Task<IActionResult> Download(string? FileName) // Returns a download page 
    {
        if (String.IsNullOrEmpty(FileName))
        {
            return BadRequest();
        }
        URLReply? reply = await backend.GetDocumentDownloadURL(FileName);
        if (reply is not null)
        {
            // return View(new URLRef(reply));
            return Redirect(reply.URL);
        }
        return NotFound();
    }

    // GET: /Document/Upload
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload([Bind()] UploadableDocument? uploadableDocument)
    {
        Console.WriteLine("Hit Upload (POST)");
        if (uploadableDocument is null)
        {
            Console.WriteLine("Document is null.");
            return BadRequest();
        }
        if (uploadableDocument.File is null)
        {
            Console.WriteLine("Uploaded Document is null.");
            return BadRequest();
        }
        List<Tag>? tags = uploadableDocument.ConvertTagToList() ?? new List<Tag>(0);
        await backend.UploadDocument(uploadableDocument.FileName, uploadableDocument.ToBytes(), tags);
        return RedirectToAction(nameof(Index));
    }

    // GET: /Document/Edit/{FileName}
    [HttpGet]
    public async Task<IActionResult> Edit(string? FileName) // Returns a page that enables editing
    {
        DocumentRef? document = await GetDocument(FileName);
        if (document is null)
        {
            return NotFound();
        }
        return View(new EditableDocument(document));
    }

    // POST: /Document/Edit/{FileName}
    [HttpPost]
    public async Task<IActionResult> Edit(string? FileName, [Bind()] EditableDocument? doc)
    {
        if (String.IsNullOrEmpty(FileName) || doc is null || String.IsNullOrEmpty(doc.FileName) || !FileName.Equals(doc.FileName))
            return BadRequest();
        if (ModelState.IsValid)
        {
            if (doc.WasRenamed())
            {
                await backend.UpdateName(doc.FileName, doc.NewFileName);
            }
            List<Tag>? newTags = doc.ConvertTagToList();
            if (newTags is null)
            {
                return BadRequest();
            }
            await backend.UpdateTags(doc.NewFileName, newTags);
        }
        return RedirectToAction(nameof(Index));
    }

    // GET: Document/Delete/{FileName}
    public async Task<IActionResult> Delete(string? FileName)
    {
        DocumentRef? documentRef = await GetDocument(FileName);
        if (documentRef is null)
            return NotFound();
        return View(documentRef);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string? FileName)
    {
        if (String.IsNullOrEmpty(FileName))
            return NotFound();
        await backend.DeleteDocument(FileName);
        return RedirectToAction(nameof(Index));
    }





    private async Task<DocumentRef?> GetDocument(string? FileName)
    {
        if (String.IsNullOrEmpty(FileName))
            return null;
        return await backend.GetDocument(FileName);
    }
}