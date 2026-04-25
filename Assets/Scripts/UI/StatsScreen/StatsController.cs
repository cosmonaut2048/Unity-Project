using System.Collections.Generic;
using System.Linq;
using Content;
using Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.StatsScreen
{
    public class StatsController : MonoBehaviour
    {
        private DailyReport _report;
        
        // Контейнер портретов.
        private VisualElement _workersContainer;
        
        // Элементы прогресс-бара.
        private VisualElement _progressOld;
        private VisualElement _progressNew;
        private Label _progressBarText;
        
        // Отчёт по кофе.
        // Контейнер потреблённого кофе.
        private VisualElement _coffeeConsumedContainer;
        // Контейнер полученного кофе.
        private VisualElement _coffeeObtainedContainer;
        
        // Отчёт по перерывам.
        // Контейнер использованных перерывов.
        private VisualElement _breaksTakenContainer;
        
        // Отчёт по оставшимся ресурсам.
        // Контейнер оставшегося кофе.
        private VisualElement _coffeeLeftContainer;
        // Контейнер оставшихся перерывов.
        private VisualElement _breaksLeftContainer;
        // Оставшиеся дни до конца недели.
        private Label _daysLeft;

        void Start()
        {
            _report = OfficeRuntime.Instance.DailyReport;
            var root = GetComponent<UIDocument>().rootVisualElement;
            
            // Queue:
            _workersContainer = root.Q<VisualElement>("Workers_Container");
            
            _progressOld = root.Q<VisualElement>("progress_old");
            _progressNew = root.Q<VisualElement>("progress_new");
            _progressBarText = root.Q<Label>("progress_bar_text");
            
            _coffeeConsumedContainer = root.Q<VisualElement>("Coffee_Consumed_Container");
            _coffeeObtainedContainer = root.Q<VisualElement>("Coffee_Obtained_Container");
            
            _breaksTakenContainer = root.Q<VisualElement>("Breaks_Taken_Container");
            
            _coffeeLeftContainer = root.Q<VisualElement>("Coffee_Left_Container");
            _breaksLeftContainer = root.Q<VisualElement>("Breaks_Left_Container");
            
            _daysLeft = root.Q<Label>("days_left");
            
            // Очищаем отображаемые данные.
            ClearStats();
            
            // Отображаем данные из отчёта.
            SetPortraits();
            SetProgressBar();
            SetCoffee();
            SetBreaks();
            SetLeft();
            
        }

        private void SetPortraits()
        {
            foreach (var worker in _report.Workers)
            {
                VisualElement portraitContainer = new VisualElement { style = { flexGrow = 0 } };
                portraitContainer.AddToClassList("worker--portrait--container");

                var portraitImage = worker.IsEmployed ? 
                    new StyleBackground(worker.Worker.Appearance.IconSprite) : 
                    new StyleBackground(worker.Worker.Appearance.FiredIconSprite);
                
                VisualElement portrait = new VisualElement { style = { flexGrow = 0, backgroundImage = portraitImage } };
                
                portrait.AddToClassList("worker--portrait");
                
                // Добавляем портрет в контейнер.
                portraitContainer.Add(portrait);

                // Если уволен -- добавляем оверлей.
                if (!worker.IsEmployed)
                {
                    VisualElement firedOverlay = new VisualElement { style = { flexGrow = 0 } };
                    firedOverlay.AddToClassList("fired--overlay");
                    portraitContainer.Add(firedOverlay);
                }
                
                // Отображаем контейнер с портретом.
                _workersContainer.Add(portraitContainer);
            }
        }

        private void SetProgressBar()
        {
            _progressOld.style.width = _report.QuotaSize != 0 ? Length.Percent((float)_report.QuotaProgressOld / _report.QuotaSize * 100) : Length.Percent(0);
            _progressNew.style.width = _report.QuotaSize != 0 ? Length.Percent((float)_report.QuotaProgressNew / _report.QuotaSize * 100) : Length.Percent(0);
            _progressBarText.text = $"{_report.QuotaProgressNew}/{_report.QuotaSize}";
        }

        private void SetCoffee()
        {
            if (_report.CoffeeConsumed == 0)
            {
                AddNone(_coffeeConsumedContainer);
            }
            else
            {
                for (int i = 0; i < _report.CoffeeConsumed; i++)
                {
                    AddObjectWithStyle(_coffeeConsumedContainer, "coffee--empty");
                }
            }

            if (_report.CoffeeObtained == 0)
            {
                AddNone(_coffeeObtainedContainer);
            }
            else
            {
                for (int i = 0; i < _report.CoffeeObtained; i++)
                {
                    AddObjectWithStyle(_coffeeObtainedContainer, "coffee--full");
                }   
            }
        }

        private void SetBreaks()
        {
            if (_report.BreaksTaken == 0)
            {
                AddNone(_breaksTakenContainer);
            }
            else
            {
                for (int i = 0; i < _report.BreaksTaken; i++)
                {
                    AddObjectWithStyle(_breaksTakenContainer, "voucher");
                }
            }
        }

        private void SetLeft()
        {
            if (_report.CoffeeLeft == 0)
            {
                AddNone(_coffeeLeftContainer);
            }
            else
            {
                for (int i = 0; i < _report.CoffeeLeft; i++)
                {
                    AddObjectWithStyle(_coffeeLeftContainer, "coffee--full");
                }
            }

            if (_report.BreaksLeft == 0)
            {
                AddNone(_breaksLeftContainer);
            }
            else
            {
                for (int i = 0; i < _report.BreaksLeft; i++)
                {
                    AddObjectWithStyle(_breaksLeftContainer, "voucher");
                }
            }

            _daysLeft.text = $"{_report.DaysLeft} DAYS LEFT.";
        }

        private void ClearStats()
        {
            ClearAllContainers();
            _progressOld.style.width = Length.Percent(0);
            _progressNew.style.width = Length.Percent(0);
            _progressBarText.text = "";
            _daysLeft.text = "";
        }

        private void ClearAllContainers()
        {
            ClearContainer(_workersContainer);
            ClearContainer(_coffeeConsumedContainer);
            ClearContainer(_coffeeObtainedContainer);
            ClearContainer(_breaksTakenContainer);
            ClearContainer(_coffeeLeftContainer);
            ClearContainer(_breaksLeftContainer);
        }
        
        private void ClearContainer(VisualElement container)
        {
            List<VisualElement> elements = container?.Children().ToList() ?? new List<VisualElement>();
            
            foreach (var element in elements)
            {
                if (element != null)
                    container?.Remove(element);
            }
        }

        private void AddObjectWithStyle(VisualElement container, string style)
        {
            VisualElement element = new VisualElement { style = { flexGrow = 0 } };
            element.AddToClassList(style);
                
            container.Add(element);
        }

        private void AddNone(VisualElement container)
        {
            VisualElement none = new VisualElement { style = { flexGrow = 0 } };
            none.AddToClassList("none");
                
            container.Add(none);
        }
    }
}