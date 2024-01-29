using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestProject.Models;
using TestProject.Repository;
using TestProject.Services;

namespace TestProject.Controllers
{
    public class TestModelsController : Controller
    {
        private readonly ITestModelService _service;

        public TestModelsController(ITestModelService service)
        {
            _service = service;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _service.GetTestModels());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTestModelJsonFile(IFormFile jsonFile)
        {
            if(jsonFile != null && jsonFile.Length > 0)
            {
                using (var reader = new StreamReader(jsonFile.OpenReadStream()))
                {
                    var jsonContent = reader.ReadToEnd();
                    _service.AddTestModel(jsonContent);
                }
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTestModelTextFile(IFormFile textFile)
        {
            if (textFile != null && textFile.Length > 0)
            {
                using (var reader = new StreamReader(textFile.OpenReadStream()))
                {
                    var textContent = reader.ReadToEnd();
                    var jsonContent = ConvertTextToJson(textContent);
                    _service.AddTestModel(jsonContent);
                }
            }
            return RedirectToAction("Index");
        }
        private string ConvertTextToJson(string textContent)
        {
            JObject config = new JObject();
            string[] rows = textContent.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                string[] parts = row.Split(':');

                JToken currentToken = config;
                for (int i = 0; i < parts.Length - 2; i++)
                {
                    string key = parts[i].Trim(); 
                    if (!currentToken.Contains(key))
                    {
                        currentToken[key] = new JObject();
                    }
                    currentToken = currentToken[key];
                }
                string lastKey = parts[parts.Length - 2].Trim();
                string value = parts[parts.Length - 1].Trim();
                if (currentToken[lastKey] == null)
                {
                    currentToken[lastKey] = value;
                }
                else
                {
                    var existingValue = currentToken[lastKey];

                    if (existingValue is JObject)
                    {
                        JArray array;
                        if (existingValue.Type == JTokenType.Array)
                        {
                            array = (JArray)existingValue;
                        }
                        else
                        {
                            array = new JArray(existingValue);
                        }

                        array.Add(value);
                        currentToken[lastKey] = array;
                    }
                    else
                    {
                        var array = new JArray { existingValue, value };
                        currentToken[lastKey] = array;
                    }
                }
            }
            return JsonConvert.SerializeObject(config, Formatting.Indented);
        }



        [HttpGet("TestModels/Config/{*configPath}")]
        public IActionResult GetConfig(string configPath)
        {
            try
            {
                var config = _service.GetConfigForPath(configPath);

                if (config != null)
                {
                    return Content(config.ToString(), "application/json");
                }
                else
                {
                    return NotFound($"Configuration not found for path: {configPath}");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error processing request: {ex.Message}");
            }
        }


      

    }






    //// GET: TestModels/Details/5
    //public async Task<IActionResult> Details(int? id)
    //{
    //    if (id == null)
    //    {
    //        return NotFound();
    //    }

    //    var testModel = await _context.TestModel
    //        .FirstOrDefaultAsync(m => m.Id == id);
    //    if (testModel == null)
    //    {
    //        return NotFound();
    //    }

    //    return View(testModel);
    //}

    // GET: TestModels/Create


    //    // POST: TestModels/Create
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Create([Bind("Id,Key,Value,ParentId")] TestModel testModel)
    //    {
    //        if (ModelState.IsValid)
    //        {
    //            _context.Add(testModel);
    //            await _context.SaveChangesAsync();
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(testModel);
    //    }

    //    // GET: TestModels/Edit/5
    //    public async Task<IActionResult> Edit(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var testModel = await _context.TestModel.FindAsync(id);
    //        if (testModel == null)
    //        {
    //            return NotFound();
    //        }
    //        return View(testModel);
    //    }

    //    // POST: TestModels/Edit/5
    //    // To protect from overposting attacks, enable the specific properties you want to bind to.
    //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    //    [HttpPost]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> Edit(int id, [Bind("Id,Key,Value,ParentId")] TestModel testModel)
    //    {
    //        if (id != testModel.Id)
    //        {
    //            return NotFound();
    //        }

    //        if (ModelState.IsValid)
    //        {
    //            try
    //            {
    //                _context.Update(testModel);
    //                await _context.SaveChangesAsync();
    //            }
    //            catch (DbUpdateConcurrencyException)
    //            {
    //                if (!TestModelExists(testModel.Id))
    //                {
    //                    return NotFound();
    //                }
    //                else
    //                {
    //                    throw;
    //                }
    //            }
    //            return RedirectToAction(nameof(Index));
    //        }
    //        return View(testModel);
    //    }

    //    // GET: TestModels/Delete/5
    //    public async Task<IActionResult> Delete(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return NotFound();
    //        }

    //        var testModel = await _context.TestModel
    //            .FirstOrDefaultAsync(m => m.Id == id);
    //        if (testModel == null)
    //        {
    //            return NotFound();
    //        }

    //        return View(testModel);
    //    }

    //    // POST: TestModels/Delete/5
    //    [HttpPost, ActionName("Delete")]
    //    [ValidateAntiForgeryToken]
    //    public async Task<IActionResult> DeleteConfirmed(int id)
    //    {
    //        var testModel = await _context.TestModel.FindAsync(id);
    //        if (testModel != null)
    //        {
    //            _context.TestModel.Remove(testModel);
    //        }

    //        await _context.SaveChangesAsync();
    //        return RedirectToAction(nameof(Index));
    //    }

    //    private bool TestModelExists(int id)
    //    {
    //        return _context.TestModel.Any(e => e.Id == id);
    //    }
}

