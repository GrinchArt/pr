using TestProject.Models;

namespace TestProject.Services
{
    public interface ITestModelService
    {
        TestModel GetTestModelById(int id);
        Task<IEnumerable<TestModel>> GetTestModels();
        void AddTestModel(string jsonContent);
        object GetConfigForPath(string configPath);
    }
}
