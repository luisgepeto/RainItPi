

namespace RainIt.Domain.Repository
{
    public class MyFirstTestModel
    {
        public int MyFirstTestModelId { get; set; }
        public string PropertyOne { get; set; }
        public string PropertyTwo { get; set; }
        public int MySecondTestModelId { get; set; }
        public virtual MySecondTestModel MySecondTestModel { get; set; }
       
    }
}
