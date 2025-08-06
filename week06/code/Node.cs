public class Node
{
    public int Data { get; set; }
    public Node? Right { get; private set; }
    public Node? Left { get; private set; }

    public Node(int data)
    {
        this.Data = data;
    }

    public void Insert(int value)
    {
        // TODO Start Problem 1

        if (value < Data) // If the value is less, go left
        {
            // Insert to the left
            if (Left is null)
                Left = new Node(value);
            else
                Left.Insert(value);
        }
        else if (value > Data) // If the value is greater, go right
        {
            // Insert to the right
            if (Right is null)
                Right = new Node(value);
            else
                Right.Insert(value);
        }
        // If value equals Data, do nothing (no duplicates allowed)
    }

    public bool Contains(int value)
    {
        // TODO Start Problem 2
        if (value == Data)
        {
            return true; // Found the value in the current node
        }
        if (value < Data)
        {
            // Value is smaller, search left subtree
            if (Left is null)
                return false; 
            else
                return Left.Contains(value); // Keep searching left
        }
        else
        {
            // Value is greater, search right subtree
            if (Right is null)
                return false; 
            else
                return Right.Contains(value); // Keep searching right
        }
    }

    public int GetHeight()
    {
        // TODO Start Problem 4
        
        // Get the height of the left subtree (0 if null)
        int leftHeight = Left is null ? 0 : Left.GetHeight();

        // Get the height of the right subtree (0 if null)
        int rightHeight = Right is null ? 0 : Right.GetHeight();

        // Return 1 + the larger of the two heights
        return 1 + Math.Max(leftHeight, rightHeight);
    }
}