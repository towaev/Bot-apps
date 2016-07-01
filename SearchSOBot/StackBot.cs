using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SearchSOBot
{

    public class StackBot
    {
        public Item[] items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }

    public class Item
    {
        public string[] tags { get; set; }
        public int question_score { get; set; }
        public bool is_accepted { get; set; }
        public bool has_accepted_answer { get; set; }
        public int answer_count { get; set; }
        public bool is_answered { get; set; }
        public int question_id { get; set; }
        public string item_type { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int creation_date { get; set; }
        public string body { get; set; }
        public string excerpt { get; set; }
        public string title { get; set; }
    }

}