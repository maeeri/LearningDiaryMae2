using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningDiaryMae2.Controllers;
using LearningDiaryMae2.Data;
using LearningDiaryMae2.Models;

namespace LearningDiaryMae2.Views
{
    public class DiaryTopicsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DiaryTopicsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: DiaryTopics
        public async Task<IActionResult> Index()
        {
            var viewModel = new ViewModel();
            viewModel.Topics = await GetTopics();

            return View(viewModel);
        }

        // GET: DiaryTopics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewModel viewModel = new ViewModel();
            viewModel.Topics = await GetTopics();
            viewModel.Topic = viewModel.Topics.FirstOrDefault(m => m.Id == id);

            if (viewModel.Topic == null)
            {
                return NotFound();
            }
            
            viewModel.Tasks = await GetTasks(id);

            return View(viewModel);
        }

        // GET: DiaryTopics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: DiaryTopics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewModel viewModel)
        {
            viewModel.Topic.LastEditDate = DateTime.Now;
            TimeSpan timeSpent = new TimeSpan();

            if (viewModel.Topic.CompletionDate != null)
            {
                viewModel.Topic.InProgress = Methods.DateCheck((DateTime)viewModel.Topic.CompletionDate, DateTime.Now.Date);
                timeSpent = (TimeSpan)(viewModel.Topic.CompletionDate - viewModel.Topic.StartLearningDate);
            }

            else
            {
                viewModel.Topic.InProgress = true;
                timeSpent = DateTime.Now.Date - viewModel.Topic.StartLearningDate;
            }

            viewModel.Topic.TimeSpent = timeSpent.Days;
            if (ModelState.IsValid)
            {
                _context.Add(viewModel.Topic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: DiaryTopics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new ViewModel();
            viewModel.Topic = await _context.DiaryTopic.FindAsync(id);

            if (viewModel.Topic == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        // POST: DiaryTopics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViewModel viewModel)
        {

            if (id != viewModel.Topic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TimeSpan timeSpent = new TimeSpan();

                    if (viewModel.Topic.CompletionDate != null)
                    {
                        viewModel.Topic.InProgress = ClassLibraryDateMethods.Class1.FutureDate((DateTime)viewModel.Topic.CompletionDate);
                        timeSpent = (TimeSpan)(viewModel.Topic.CompletionDate - viewModel.Topic.StartLearningDate);
                    }

                    else
                    {
                        viewModel.Topic.InProgress = true;
                        timeSpent = DateTime.Now.Date - viewModel.Topic.StartLearningDate;
                    }

                    viewModel.Topic.TimeSpent = timeSpent.Days;
                    viewModel.Topic.LastEditDate = DateTime.Now;
                    _context.DiaryTopic.Update(viewModel.Topic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaryTopicExists(viewModel.Topic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: DiaryTopics/Delete/5
        //deletes a topic and related tasks
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var viewModel = new ViewModel();
            viewModel.Topic = await _context.DiaryTopic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (viewModel.Topic == null)
            {
                return NotFound();
            }

            viewModel.Topic = await _context.DiaryTopic.FindAsync(id);
            var taskList = _context.DiaryTask.Where(x => x.DiaryTopic == viewModel.Topic.Id).ToList();

            _context.DiaryTopic.Remove(viewModel.Topic);
            _context.DiaryTask.RemoveRange(taskList);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Search()
        {
            return View();
        }

        //returns results of a topic title search
        public async Task<IActionResult> SearchResults(string searchString)
        {
            var topics = from m in _context.DiaryTopic
                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                var viewModel = new ViewModel();
                viewModel.Topics = topics.Where(t => t.Title!.Contains(searchString)).ToList();
                return View(viewModel);
            }

            return NotFound();
        }

        private bool DiaryTopicExists(int id)
        {
            return _context.DiaryTopic.Any(e => e.Id == id);
        }

        //gets all diarytopics from database
        public async Task<List<DiaryTopic>> GetTopics()
        {
            var topics = _context.DiaryTopic.Select(topic => topic).ToList();
            return topics;
        }

        // GET: DiaryTasks/Create
        public async Task<IActionResult> CreateTask(int topicId)
        {
            var viewModel = new ViewModel();
            DiaryTopicsController topContr = new DiaryTopicsController(_context);
            viewModel.Topics = await topContr.GetTopics();
            viewModel.Topic = viewModel.Topics.FirstOrDefault(t => t.Id == topicId);

            return View(viewModel);
        }

        // POST: DiaryTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(ViewModel viewModel)
        {
            viewModel.Task.DiaryTopic = viewModel.Topic.Id;

            if (viewModel.Task.Title != null)
            {
                _context.DiaryTask.Add(viewModel.Task);

                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "DiaryTopics");
            }
            return View(viewModel);
        }

        // GET: DiaryTasks/Edit/5
        public async Task<IActionResult> EditTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var viewModel = new ViewModel();
            viewModel.Task = await _context.DiaryTask.FindAsync(id);
            if (viewModel.Task == null)
            {
                return NotFound();
            }
            return View(viewModel);
        }

        // POST: DiaryTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, ViewModel viewModel)
        {
            if (id != viewModel.Task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.DiaryTask.Update(viewModel.Task);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiaryTaskExists(viewModel.Task.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "DiaryTopics");
            }
            return View(viewModel);
        }

        private bool DiaryTaskExists(int id)
        {
            return _context.DiaryTask.Any(e => e.Id == id);
        }

        //gets all diarytasks related to a specific diarytopic
        public async Task<List<DiaryTask>> GetTasks(int? topicId)
        {
            var tasks = _context.DiaryTask.Where(task => task.DiaryTopic.Equals(topicId)).Select(task => task).ToList();
            return tasks;
        }
    }
}
