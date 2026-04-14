using System.Collections.Generic;
using System.Linq;
using Content;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI.StatsScreen
{
    public class StatsController : MonoBehaviour
    {
        [SerializeField] private DailyReport report;
        
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
            foreach (var worker in report.Workers)
            {
                VisualElement portraitContainer = new VisualElement { style = { flexGrow = 0 } };
                portraitContainer.AddToClassList("worker--portrait--container");

                var portraitImage = worker.IsEmployed ? 
                    new StyleBackground(worker.Appearance.IconSprite) : 
                    new StyleBackground(worker.Appearance.FiredIconSprite);
                
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
            _progressOld.style.width = Length.Percent(report.QuotaProgressOld / report.QuotaSize * 100);
            _progressNew.style.width = Length.Percent(report.QuotaProgressNew / report.QuotaSize * 100);
            _progressBarText.text = $"{report.QuotaProgressNew}/{report.QuotaSize}";
        }

        private void SetCoffee()
        {
            for (int i = 0; i < report.CoffeeConsumed; i++)
            {
                VisualElement coffeeEmpty = new VisualElement { style = { flexGrow = 0 } };
                coffeeEmpty.AddToClassList("coffee--empty");
                
                _coffeeConsumedContainer.Add(coffeeEmpty);
            }

            for (int i = 0; i < report.CoffeeObtained; i++)
            {
                VisualElement coffeeFull = new VisualElement { style = { flexGrow = 0 } };
                coffeeFull.AddToClassList("coffee--full");
                
                _coffeeObtainedContainer.Add(coffeeFull);
            }
        }

        private void SetBreaks()
        {
            for (int i = 0; i < report.BreaksTaken; i++)
            {
                VisualElement breakVoucher = new VisualElement { style = { flexGrow = 0 } };
                breakVoucher.AddToClassList("voucher");
                
                _breaksTakenContainer.Add(breakVoucher);
            }
        }

        private void SetLeft()
        {
            for (int i = 0; i < report.CoffeeLeft; i++)
            {
                VisualElement coffeeFull = new VisualElement { style = { flexGrow = 0 } };
                coffeeFull.AddToClassList("coffee--full");
                
                _coffeeLeftContainer.Add(coffeeFull);
            }

            for (int i = 0; i < report.BreaksLeft; i++)
            {
                VisualElement breakVoucher = new VisualElement { style = { flexGrow = 0 } };
                breakVoucher.AddToClassList("voucher");
                
                _breaksLeftContainer.Add(breakVoucher);
            }
            
            _daysLeft.text = $"{report.DaysLeft} DAYS LEFT.";
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
    }
}