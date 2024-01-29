using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using TestProject.Models;
using TestProject.Repository;

namespace TestProject.Services
{
    
    public class TestModelService:ITestModelService
    {
        private readonly ITestModelRepository _repository;
        public TestModelService(ITestModelRepository repository)
        {
            _repository=repository;
        }

        public void AddTestModel(string jsonContent)
        {          
            try
            {
                var token = JToken.Parse(jsonContent);

                if (token.Type == JTokenType.Array)
                {
                    var models = token.ToObject<List<TestModel>>();
                    foreach (var model in models)
                    {
                        _repository.AddTestModel(model);
                    }
                }
                else if (token.Type == JTokenType.Object)
                {
                    var singleModel = token.ToObject<TestModel>();
                    _repository.AddTestModel(singleModel);
                }
                else
                {
                    throw new InvalidOperationException("Invalid JSON format");
                }
            }
            catch (JsonSerializationException ex)
            {
                throw new InvalidOperationException("Error deserializing JSON", ex);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Error processing JSON file", ex);
            }
        }

        public TestModel GetTestModelById(int id)
        {
            return _repository.GetTestModelById(id);
        }
        
        public async Task<IEnumerable<TestModel>> GetTestModels()
        {
            return _repository.GetTestModels();
        }

        public object GetConfigForPath(string configPath)
        {
            var pathSegments = configPath.Split('/');
            var currentNode =  _repository.GetRoot(pathSegments[0]);

            for (int i = 1; i < pathSegments.Length && currentNode != null; i++)
            {
                currentNode = currentNode.Children.FirstOrDefault(c => c.Key == pathSegments[i]);
            }

            if (currentNode == null)
            {
                throw new NotFoundException($"Configuration not found for path: {configPath}");
            }

            if (currentNode.Children.Any())
            {
                var subtree = _repository.GetSubtree(currentNode.Id);
                return subtree;
            }
            else
            {
                return new { Key = currentNode.Key, Value = currentNode.Value };
            }
        }



    }
}
