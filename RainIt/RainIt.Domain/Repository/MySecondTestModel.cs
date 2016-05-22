

using System.Collections;
using System.Collections.Generic;

namespace RainIt.Domain.Repository
{
    public class MySecondTestModel
    {
        public int MySecondTestModelId { get; set; }
        public string PropertyOne { get; set; }
        public IEnumerable<MyFirstTestModel> MyTestModels { get; set; }
    }
}
