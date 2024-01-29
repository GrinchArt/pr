namespace TestProject.Models
{
    public class TestModel
    {
        public int Id { get; set; } 
        public string Key { get; set; }
        public string? Value { get; set; }
        public int? ParentId {  get; set; }

        public TestModel Parent {  get; set; }
        public List<TestModel> Children { get; set; }
    }
}
