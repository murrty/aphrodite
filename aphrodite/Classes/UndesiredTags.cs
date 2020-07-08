using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aphrodite {
    class UndesiredTags {
        public static readonly string[] HardcodedUndesiredTags = { "avoid_posting", "conditional_dnp", "sound_warning", "unknown_artist_signature" };

        /// <summary>
        /// Determine if a tag is undesired
        /// </summary>
        /// <param name="tag">The tag string</param>
        /// <returns></returns>
        public static bool isUndesiredHardcoded(string tag) {
            for (int i = 0; i < HardcodedUndesiredTags.Length; i++) {
                if (tag == HardcodedUndesiredTags[i])
                    return true;
            }

            return false;
        }

        public static bool isUndesired(string tag) {
            List<string> undesiredTags = new List<string>();
            undesiredTags.AddRange(General.Default.undesiredTags.Split(' '));

            for (int i = 0; i < undesiredTags.Count; i++) {
                if (tag == undesiredTags[i])
                    return false;
            }

            return false;
        }
    }
}
