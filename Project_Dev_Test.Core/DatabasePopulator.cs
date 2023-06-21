﻿using Project_Dev_Test.Core.Entities;
using Project_Dev_Test.Core.Interfaces;
using System.Linq;

namespace Project_Dev_Test.Core
{
    public class DatabasePopulator
    {
        public static int PopulateDatabase(IRepository todoRepository)
        {
            if (todoRepository.List<ToDoItem>().Count() >= 5) return 0;

            todoRepository.Add(new ToDoItem
            {
                Title = "Get Sample Working",
                Description = "Try to get the sample to build."
            });
            todoRepository.Add(new ToDoItem
            {
                Title = "Review Solution",
                Description = "Review the different projects in the solution and how they relate to one another."
            });
            todoRepository.Add(new ToDoItem
            {
                Title = "Run and Review Tests",
                Description = "Make sure all the tests run and review what they are doing."
            });

            return todoRepository.List<ToDoItem>().Count;
        }
    }
}