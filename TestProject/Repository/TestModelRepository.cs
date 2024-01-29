using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.Repository
{
    public class TestModelRepository : ITestModelRepository
    {
        private readonly TestProjectDbContext _context;

        public TestModelRepository(TestProjectDbContext context)
        {
            _context = context;
        }

        public void AddTestModel(TestModel model)
        {
            _context.TestModel.Add(model);
            _context.SaveChanges();
        }


        public  TestModel GetTestModelById(int id)
        {
            return _context.TestModel.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<TestModel> GetTestModels()
        {
           return _context.TestModel.ToList();
        }


        public TestModel GetRoot(string key)
        {
            return _context.TestModel.FirstOrDefault(x=>x.Key == key && x.ParentId==null);
        } 

        public List<TestModel> GetSubtree(int rootId)
        {
            return _context.TestModel.Where(x=>x.Id==rootId)
                .Include(z=>z.Children)
                .ToList();
        }

    }
}
