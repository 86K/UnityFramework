using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityFramework.Runtime
{
    public class CalendarController : MonoBehaviour
    {
        [SerializeField] private GameObject _calendarPanel;
        [SerializeField] private Text _yearNumText;
        [SerializeField] private Text _monthNumText;
        [SerializeField] private Text _dayNumText;
        [SerializeField] private GameObject _item;

        [Space] [SerializeField] private Button _yearPrevBtn;
        [SerializeField] private Button _yearNextBtn;
        [SerializeField] private Button _monthPrevBtn;
        [SerializeField] private Button _monthNextBtn;
        [SerializeField] private Button _resetBtn;
        [SerializeField] private Button _confirmBtn;

        private List<GameObject> _dateItems = new List<GameObject>();
        const int _totalDateNum = 42;

        private DateTime _dateTime;
        private Button _selectItemBtn = null;
        private Color _normalColor = new Color(246, 82, 117, 0) / 255;
        private Color _selectColor = new Color(246, 82, 117, 255) / 255;

        private void Awake()
        {
            _calendarPanel.SetActive(false);

            OnClickYearPrevBtn();
            OnClickYearNextBtn();
            OnClickMonthPrevBtn();
            OnClickMonthNextBtn();
        }

        void Start()
        {
            Vector3 startPos = _item.transform.localPosition;
            _dateItems.Clear();
            _dateItems.Add(_item);

            for (int i = 1; i < _totalDateNum; i++)
            {
                GameObject item = GameObject.Instantiate(_item, _item.transform.parent, true);
                item.name = "Item" + (i + 1);
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.identity;
                // NOTE：这里是计算出来的，x|y的spacing需要手动调整
                item.transform.localPosition =
                    new Vector3((i % 7) * 35 + startPos.x, startPos.y - (i / 7) * 25, startPos.z);

                _dateItems.Add(item);
            }

            _dateTime = DateTime.Now;
            _dayNumText.text = _dateTime.Day.ToString();

            CreateCalendar();

            _calendarPanel.SetActive(false);
        }

        void CreateCalendar()
        {
            DateTime firstDay = _dateTime.AddDays(-(_dateTime.Day - 1));
            int index = GetDays(firstDay.DayOfWeek);

            int date = 0;
            for (int i = 0; i < _totalDateNum; i++)
            {
                Text label = _dateItems[i].GetComponentInChildren<Text>();
                _dateItems[i].SetActive(false);

                if (i >= index)
                {
                    DateTime thatDay = firstDay.AddDays(date);
                    if (thatDay.Month == firstDay.Month)
                    {
                        _dateItems[i].SetActive(true);
                        label.text = (date + 1).ToString();
                        date++;
                    }
                }
            }

            _yearNumText.text = _dateTime.Year.ToString();
            _monthNumText.text = _dateTime.Month.ToString();
        }

        int GetDays(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return 1;
                case DayOfWeek.Tuesday: return 2;
                case DayOfWeek.Wednesday: return 3;
                case DayOfWeek.Thursday: return 4;
                case DayOfWeek.Friday: return 5;
                case DayOfWeek.Saturday: return 6;
                case DayOfWeek.Sunday: return 0;
            }

            return 0;
        }

        void OnClickYearPrevBtn()
        {
            if (_yearPrevBtn != null)
            {
                _yearPrevBtn.onClick.AddListener(delegate
                {
                    _dateTime = _dateTime.AddYears(-1);
                    CreateCalendar();
                });
            }
        }

        void OnClickYearNextBtn()
        {
            if (_yearNextBtn != null)
            {
                _yearNextBtn.onClick.AddListener(delegate
                {
                    _dateTime = _dateTime.AddYears(1);
                    CreateCalendar();
                });
            }
        }

        void OnClickMonthPrevBtn()
        {
            if (_monthPrevBtn != null)
            {
                _monthPrevBtn.onClick.AddListener(delegate
                {
                    _dateTime = _dateTime.AddMonths(-1);
                    CreateCalendar();
                });
            }
        }

        void OnClickMonthNextBtn()
        {
            if (_monthNextBtn != null)
            {
                _monthNextBtn.onClick.AddListener(delegate
                {
                    _dateTime = _dateTime.AddMonths(1);
                    CreateCalendar();
                });
            }
        }
        

        /// <summary>
        /// 得到完整的日期，格式：yyyy-MM-dd
        /// </summary>
        string GetFullDateString()
        {
            return DateTime.Parse(_yearNumText.text + "-" + _monthNumText.text + "-" + _dayNumText.text)
                .ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 日期天数项点击事件
        /// </summary>
        public void OnDateItemClick(Button selfBtn)
        {
            if (_selectItemBtn != null)
            {
                _selectItemBtn.image.color = _normalColor;
            }

            _selectItemBtn = selfBtn;
            _dayNumText.text = selfBtn.GetComponentInChildren<Text>().text;
            _selectItemBtn.image.color = _selectColor;
        }
    }
}