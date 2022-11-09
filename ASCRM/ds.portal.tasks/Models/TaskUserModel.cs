using System;

namespace ds.portal.tasks.Models
{
    public class TaskUserModel
    {
        private string _color;

        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string Color 
        {
            get 
            {
                if (string.IsNullOrEmpty(_color))
                {
                    var random = new Random(UserId);
                    _color = string.Format("#{0:X6}", random.Next(0x1000000));
                }
                return _color; 
            }
            set
            {
                _color = value;
            }
        }
    }
}
