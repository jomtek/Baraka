using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Baraka.Models.Quran.Mushaf
{
    public enum CacheOrientation
    {
        FAR_LEFT,
        FAR_RIGHT,
    }

    public class MushafPreloadedPageCache
    {
        public List<MushafPreloadedPageModel> Stack { get; private set; } = new();

        public event Action<int, CacheOrientation> PageGenerationRequested;

        public bool HasPage(int page)
        {
            foreach (var pm in Stack)
                if (pm.Page == page)
                    return true;
            return false;
        }

        public MushafPreloadedPageModel GetPage(int page)
        {
            foreach (var pm in Stack)
                if (pm.Page == page)
                    return pm;
            throw new ArgumentException($"Page {page} could not be found in the stack");
        }

        public void AddPage(int page, List<StackPanel> controls, CacheOrientation orientation = CacheOrientation.FAR_RIGHT)
        {
            if (HasPage(page))
                throw new ArgumentException($"Page {page} is already registered into the stack");
            
            var pm = new MushafPreloadedPageModel(page, controls);
            if (orientation == CacheOrientation.FAR_LEFT)
            {
                Stack.Insert(0, pm);
            }
            else
            {
               Stack.Add(pm);
            }
        }

        public void ErasePage(CacheOrientation orientation)
        {
            if (orientation == CacheOrientation.FAR_LEFT)
            {
                Stack[0] = null;
                Stack.RemoveAt(0);
            }
            else if (orientation == CacheOrientation.FAR_RIGHT)
            {

                Stack[Stack.Count - 1] = null;
                Stack.RemoveAt(Stack.Count - 1);
            }
            else
            {
                throw new NotImplementedException();
            }

            // TODO: IMPORTANT memory LEAK problem - the stack elements, despite having been removed, are not freed from
            //       memory until they are randomly collected. The memory leak can reach more than a gigabyte of RAM usage.
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }

        public void PreloadPage(CacheOrientation orientation)
        {
            if (orientation == CacheOrientation.FAR_LEFT)
            {
                var first = Stack.First();
                PageGenerationRequested?.Invoke(first.Page + 1, CacheOrientation.FAR_LEFT);
            }
            else if (orientation == CacheOrientation.FAR_RIGHT)
            {
                var last = Stack.Last();
                PageGenerationRequested?.Invoke(last.Page - 1, CacheOrientation.FAR_RIGHT);
            }
            else
            {
                throw new NotImplementedException();
            }
        }
    }
}
