using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDBContext _db;
        public CategoryController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //retriving data
            List<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }
        //craeting new action method while clicking button create a category 
        public IActionResult Create()
        {
            
            return View();
        }

        //create another method(POST) inside a categorycontroller(get and post method)
        [HttpPost]
		public IActionResult Create(Category obj)
        {
            //somecheck  obj.Nmae =obj.Displayoreder.string
            if(obj.Name == obj.DisplayOrder.ToString())
            {
              
                //method(AddModelError
                ModelState.AddModelError("name", "The DisplayOrder cannot be exactly match the Name.");
            }
			//checking validation
			if (obj.Name.ToLower() == "test")
			{
				//method(AddModelError)
				ModelState.AddModelError("","Test is an invalid value.");
			}
			if (ModelState.IsValid)
            {
                //27 lines say you have to add catrory object to category table.what we have to dsaying .
                //keeping the tarck of what its have to add.
                _db.Categories.Add(obj);
                //go to database create category.
                _db.SaveChanges();
                TempData["success"] = "Category created succeessfully";
                //Clear the ModelState to remove the values
                //ModelState.Clear();
;				//once the category is added, we want to redirect to the category index view ,it go to method INDex action
				return RedirectToAction("Index");
			}
            return View();
        }
        //copy get and post action method to create an edit page 
        //this get action method
        public IActionResult Edit(int? id)//creating a action method for editpage.[we give int? id bcz to know which value have to edit
        {
            if(id==null || id == 0) //adding if condition 
            {
                return NotFound();
            }
            //if id is valid we need to retrieve that category from database[If we retrieve one category entity framecore has helper method]
            //category object  ,we have method [find]
            Category? categoryFromDb = _db.Categories.Find(id);
            //Category? categoryFromDb1 = _db.Categories.FirstOrDefault(u=>u.CategoryId==id);
			//Category? categoryFromDb2 = _db.Categories.Where(u=>u.CategoryId==id).FirstOrDefault();
			if (categoryFromDb == null)
            {
                return NotFound();
            }
            return View(categoryFromDb);
        }
        //we retrive object and update category
        [HttpPost]
		public IActionResult Edit(Category obj) //Edit post
		{
           
            if (ModelState.IsValid)
            {
                //create category table using entity framework core.
                //_db.Categories.Add(obj);
                _db.Categories.Update(obj); 
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id== null ||id == 0)
            {
                return NotFound();
            }
            Category? categoryFromDb = _db.Categories.Find(id);

            if (categoryFromDb == null)
            {
                return NotFound();

            }
            return View(categoryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id); //to delete we need find cateogry from database
            if(obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
			TempData["success"] = "Category deleted successfully";
			return RedirectToAction("Index");

          
        }

    }
}
