using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Linq;
using System.Collections.Generic;

namespace MyLanguages.Wpf.Helpers
{
    internal static class VisualTreeHelpers
    {
        public static string GetContextByName(this DependencyObject dependencyObject)
        {
            string result = "";
            if(dependencyObject != null)
            {
                if(dependencyObject is UserControl || dependencyObject is Window)
                    result = dependencyObject.FormatForKey(true);
                else
                {
                    var parent = dependencyObject is Visual || dependencyObject is Visual3D ? VisualTreeHelper.GetParent(dependencyObject) : LogicalTreeHelper.GetParent(dependencyObject);
                    result = GetContextByName(parent);
                }
            }

            return string.IsNullOrEmpty(result) ? dependencyObject?.FormatForKey(true) ?? string.Empty : result;
        }

        public static T GetChildOfType<T>(this DependencyObject dependencyObject)
            where T : DependencyObject
        {
            if (dependencyObject == null)
                return null;

            for(int i = 0; i < VisualTreeHelper.GetChildrenCount(dependencyObject); i++)
            {
                var child = VisualTreeHelper.GetChild(dependencyObject, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null)
                    return result;
            }

            return null;
        }

        public static DependencyObject GetLastParent(this FrameworkElement element)
        {
            FrameworkElement lastElement = null;

            // While the element has a parent
            while(element.Parent != null)

                // If the last element is null
                if (lastElement == null)
                    lastElement = element.Parent as FrameworkElement; // Set element as last element

                // Otherwise...
                else
                    // If the last element parent is not null
                    if (lastElement.Parent != null)
                        lastElement = lastElement.Parent as FrameworkElement; // Set last element as the parent of it

                    // Otherwise, we found the last element and break the loop
                    else
                        break;

            // Return the last element
            return lastElement;
        }

        public static void SetChildNames(this DependencyObject dependencyObject)
        {
            List<Control> controls = new List<Control>();
            GetLogicalChildCollection<Control>(dependencyObject, controls);

            foreach(var control in controls)
            {
                string name = dependencyObject.GetType().FullName.Replace('.', '_') + "_" + controls.IndexOf(control);

                control.Name = name;
                if (control is ContentControl cc)
                {
                    string s = $"Content changed its name is {name}";
                }
            }

        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        public static string FormatForKey(this DependencyObject dependencyObject, bool fullName = false)
            => $"{(dependencyObject as FrameworkElement)?.Name ?? string.Empty}[{(fullName ? dependencyObject.GetType().FullName : dependencyObject.GetType().Name)}]";
    }
}
