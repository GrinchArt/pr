using TestProject.Models;

namespace TestProject.Repository
{
    public interface ITestModelRepository
    {

        void AddTestModel(TestModel model);
        TestModel GetTestModelById(int id);
        IEnumerable<TestModel> GetTestModels();
        TestModel GetRoot(string key);
        List<TestModel> GetSubtree(int rootId);
    }
}
