using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using PointerType = UnityEngine.UIElements.PointerType;

namespace UI.WorkDayScreen.AssignWorkersComponents
{
    public class DragAndDropManipulator : PointerManipulator
    {
        private readonly string _slotContainerName;
        private readonly string _slotClassName;
        private readonly Action<VisualElement, VisualElement> _onDrop;
        
        private bool _isDragging;
        
        private Vector2 _pointerStartPanel;
        private Vector2 _elementStartWorld;

        public DragAndDropManipulator(
            VisualElement target, 
            string slotContainerName,
            string slotClassName,
            Action<VisualElement, VisualElement> onDrop = null)
        {
            this.target = target;
            _onDrop = onDrop;
            _slotContainerName = slotContainerName;
            _slotClassName = slotClassName;
        }

        private void OnPointerDown(PointerDownEvent evt)
        {
            // Реагирует только на ЛКМ.
            if (evt.pointerType == PointerType.mouse && evt.button != 0) return;
            
            // Ставим выше остальных элементов.
            target.BringToFront();

            target.style.position = Position.Absolute;
            
            _isDragging = true;
            
            // Запоминаем начальные позиции для курсора и элемента.
            _pointerStartPanel = evt.position;
            _elementStartWorld = target.worldBound.position;
            
            // Захватываем курсор, даже если он покинул границы элемента.
            target.CapturePointer(evt.pointerId);
            
            evt.StopPropagation();
        }

        private void OnPointerMove(PointerMoveEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;
            
            VisualElement parent = target.parent;
            if (parent == null) return;

            Vector2 pointerCurrent = evt.position;
            Vector2 pointerDelta = pointerCurrent - _pointerStartPanel;

            Vector2 newWorld = _elementStartWorld + pointerDelta;
            Vector2 newLocal = parent.WorldToLocal(newWorld);
            
            target.style.left = newLocal.x;
            target.style.top = newLocal.y;
            
            evt.StopPropagation();
        }
        
        private void OnPointerUp(PointerUpEvent evt)
        {
            if (!_isDragging || !target.HasPointerCapture(evt.pointerId)) return;
            
            target.ReleasePointer(evt.pointerId);
            
            evt.StopPropagation();
        }

        private void OnPointerCaptureOut(PointerCaptureOutEvent evt)
        {
            if (!_isDragging) return;

            VisualElement closestSlot = FindClosestSlot(requireOverlap: true);

            if (closestSlot == null) SnapBackToStart();

            SnapToSlotCenter(closestSlot);

            _isDragging = false;
            _onDrop?.Invoke(target, closestSlot);
        }

        private VisualElement FindClosestSlot(bool requireOverlap)
        {
            if (target.panel == null) return null;

            VisualElement root = target.panel.visualTree;
            VisualElement slotsRoot = string.IsNullOrEmpty(_slotContainerName) ? root : root.Q<VisualElement>(_slotContainerName);
            
            if (slotsRoot == null) return null;
            
            List<VisualElement> slots = slotsRoot.Query<VisualElement>(className: _slotClassName).ToList();
            
            if (slots.Count == 0) return null;
            
            VisualElement closestSlot = null;
            float bestDistanceSq = float.MaxValue;

            foreach (VisualElement slot in slots)
            {
                if (requireOverlap && !target.worldBound.Overlaps(slot.worldBound)) continue;
                
                float distanceSq = (slot.worldBound.center - target.worldBound.center).sqrMagnitude;

                if (distanceSq < bestDistanceSq)
                {
                    bestDistanceSq = distanceSq;
                    closestSlot = slot;
                }
            }
            
            return closestSlot;
        }

        private void SnapToSlotCenter(VisualElement slot)
        {
            if (target.parent == null) return;
            
            Vector2 slotCenterWorld = slot.worldBound.center;
            Vector2 itemSize = new Vector2(target.resolvedStyle.width, target.resolvedStyle.height);

            Vector2 desiredWorld = slotCenterWorld - (itemSize * 0.5f);
            Vector2 desiredLocal = target.parent.WorldToLocal(desiredWorld);
            
            target.style.left = desiredLocal.x;
            target.style.top = desiredLocal.y;
        }

        private void SnapBackToStart()
        {
            if (target.parent == null) return;
            
            Vector2 localStart = target.parent.WorldToLocal(_elementStartWorld);
            target.style.left = localStart.x;
            target.style.top = localStart.y;
        }
        
        protected override void RegisterCallbacksOnTarget()
        {
            target.RegisterCallback<PointerDownEvent>(OnPointerDown);
            target.RegisterCallback<PointerUpEvent>(OnPointerUp);
            target.RegisterCallback<PointerMoveEvent>(OnPointerMove);
            target.RegisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }

        protected override void UnregisterCallbacksFromTarget()
        {
            target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
            target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
            target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
            target.UnregisterCallback<PointerCaptureOutEvent>(OnPointerCaptureOut);
        }
    }
}