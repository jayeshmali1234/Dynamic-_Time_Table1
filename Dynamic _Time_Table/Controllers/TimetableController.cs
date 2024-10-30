using Dynamic__Time_Table.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

public class TimetableController : Controller
{
    public ActionResult Index()
    {
        return View(new TimetableModel());
    }

    [HttpPost]
    public ActionResult CalculateTotalHours(TimetableModel model)
    {
        if (ModelState.IsValid)
        {
            model.Subjects = Enumerable.Range(1, model.TotalSubjects)
                .Select(i => new Subject { Name = $"Subject {i}" })
                .ToList();
            return View("SetSubjectHours", model);
        }
        return View("SetSubjectHours", model);
    }

    [HttpPost]
    public ActionResult GenerateTimetable(TimetableModel model)
    {
        if (model.Subjects.Sum(s => s.Hours) != model.TotalHours)
        {
            ModelState.AddModelError("", "Total subject hours must equal the total hours for the week.");
            return View("SetSubjectHours", model);
        }

        var timetable = GenerateDynamicTimetable(model);

        if (timetable == null || !timetable.Any())
        {
            ModelState.AddModelError("", "Timetable generation failed. Please check your input values.");
            return View("SetSubjectHours", model);
        }

        ViewBag.Timetable = timetable;

        return View("GeneratedTimetable", model);
    }

    private List<List<string>> GenerateDynamicTimetable(TimetableModel model)
    {
        var timetable = new List<List<string>>();
        var subjectQueue = new Queue<string>();

        // Populate the queue with each subject repeated according to its allocated hours
        foreach (var subject in model.Subjects)
        {
            for (int i = 0; i < subject.Hours; i++)
            {
                subjectQueue.Enqueue(subject.Name);
            }
        }

        int totalCells = model.WorkingDays * model.SubjectsPerDay;

        // Refill the queue if it's not large enough to fill the entire timetable grid
        while (subjectQueue.Count < totalCells)
        {
            foreach (var subject in model.Subjects)
            {
                for (int i = 0; i < subject.Hours && subjectQueue.Count < totalCells; i++)
                {
                    subjectQueue.Enqueue(subject.Name);
                }
            }
        }

        // Check if subjectQueue has enough items before generating the timetable
        if (subjectQueue.Count < totalCells)
        {
            return timetable; // Returns an empty timetable if subjectQueue is insufficient
        }

        // Generate the timetable row by row
        for (int i = 0; i < model.SubjectsPerDay; i++)
        {
            var row = new List<string>();
            for (int j = 0; j < model.WorkingDays; j++)
            {
                row.Add(subjectQueue.Dequeue());
            }
            timetable.Add(row);
        }

        return timetable;

    }
}
