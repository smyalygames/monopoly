using System.Collections.Generic;
using System.Linq;

public static class MergeMethod
{
	public static List<Property> MergeSort(List<Property> unsorted)
	{	
		
		if (unsorted.Count <= 1) //Checks if the list is longer than 1 to do a merge sort
		{
			return unsorted; //Stops the function if the length is 1 or less
		}

		int middle = unsorted.Count / 2; //Does an integer division of 2

		List<Property> left = new List<Property>(); //Creates a list for the left items in the list.
		List<Property> right = new List<Property>(); //Creates a list for the right items in the list.


		for (int i = 0; i < middle; i++) //Adds the left half of the unsorted list to the left list.
		{
			left.Add(unsorted[i]);
		}

		for (int i = middle; i < unsorted.Count; i++) //Adds the rest of the unsorted list to the right list.
		{
			right.Add(unsorted[i]);
		}
		
		//Uses recursion to get to return early.
		left = MergeSort(left);
		right = MergeSort(right);
		
		//Merges the lists.
		return Merge(left, right);
	}

	private static List<Property> Merge(List<Property> left, List<Property> right)
	{
		List<Property> sorted = new List<Property>(); //Creates the list with the sort.

		while (left.Count > 0 || right.Count > 0) //While operates as the left and right lists aren't empty.
		{
			if (left.Count > 0 && right.Count > 0) //Checks if none of the lists are empty.
			{
				if (left.First().property_id <= right.First().property_id) //Checks if the left one is smaller than the right one.
				{
					sorted.Add(left.First());
					left.Remove(left.First());
				}
				else //If the right one is smaller than the left one then...
				{
					sorted.Add(right.First());
					right.Remove(right.First());
				}
			}
			else if (left.Count > 0) //Runs if the only list left is the left one.
			{
				sorted.Add(left.First());
				left.Remove(left.First());
			}
			else if (right.Count > 0) //Runs if the only list left is the right one.
			{
				sorted.Add(right.First());
				right.Remove(right.First());
			}
		}

		return sorted; //Returns the sorted list.
	}
}
