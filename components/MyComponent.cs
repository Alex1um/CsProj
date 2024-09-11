public class MyComponent {
    public int store { get; set; }

    public MyComponent(int v) {
        store = v;
    }

    public void Print() {
        Console.WriteLine(store);
    }
}