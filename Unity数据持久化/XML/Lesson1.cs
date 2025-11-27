using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class Lesson1Test {
    [XmlElement("Test")]       //给节点进行改名称
    public int testPublic = 10;
    private int testPrivate = 11;
    protected int testProtected = 12;
    internal int testInternal = 13;

    public string testPublicStr = "123";
    public int testPro { get; set; }
    public Lesson1Test2 testClass = new Lesson1Test2();
    //结论：只有public的成员才能存储

    public int[] arrayInt = new int[3] { 5, 6, 7 };
    [XmlArray("IntList")]       //修改链表头结点名称
    [XmlArrayItem("Int32")]     //修改链表子节点名称
    public List<int> listInt = new List<int>() { 1, 2, 3, 4 };
    public List<Lesson1Test2> listItem = new List<Lesson1Test2>() {  new Lesson1Test2(), new Lesson1Test2()};
    //数组和List也能存储

    //报错 因为不支持字典
    //public Dictionary<int, string> testDic = new Dictionary<int, string>() { {1,"123" } };
}

public class Lesson1Test2 {
    [XmlAttribute("Test1")]        //这个字段，转为属性处理
    public int test1 = 1;
    [XmlAttribute()]
    public float test2 = 1.1f;
    [XmlAttribute()]
    public bool test3 = true;
}

public class Lesson1 : MonoBehaviour
{
    void Start()
    {
        # region Xml序列化
        Lesson1Test lt = new Lesson1Test();

        string path = Application.persistentDataPath + "/Lesson1Test.xml";
        print(path);
        //括号内的代码：写入一个文件流，如果有该文件，直接打开并修改，如果没有该文件，直接新建一个文件
        //using 的新用法，括号当中包裹的声明的对象 会在 大括号语句块结束后，自动释放掉
        //当语句块结束 会自动帮我们调用 对象的 Dispose方法，让其进行销毁
        // using 一般是配合 内存占用比较大 或者 有读写操作时 进行使用的
        using (StreamWriter stream = new StreamWriter(path)) {
            //产生一个序列话的机器
            XmlSerializer s = new XmlSerializer(typeof(Lesson1Test));
            //将类对象翻译为Xml文件，写入到对应的文件中
            //第一个参数：文件流对象
            //第二个参数：对象
            //注意：翻译机器必须要与翻译对象类型是一致的
            s.Serialize(stream, lt);
        }
        #endregion

        #region 自定义节点名和属性的添加
        //通过添加特性 [XmlAttribute()]来处理 将节点改为属性
        //[XmlElement("123")] 将节点名称改为123
        //[XmlArray("IntList")]     修改链表头结点名称为IntList
        //[XmlArrayItem("Int32")]   修改链表子节点名称为Int32
        #endregion

        #region 反序列化
        //判断文件是否存在
        if (File.Exists(path)) {
            using (StreamReader reader = new StreamReader(path)) {
                //产生一个反序列化的机器
                XmlSerializer s = new XmlSerializer(typeof(Lesson1Test));
                Lesson1Test lt2 = s.Deserialize(reader) as Lesson1Test; 
                //注意这里有一个问题，如果对象有List属性，并且默认有值，那么反序列化对象后，会为这个List属性添加读取到的值，并不是覆盖原有的值。其他的则是覆盖，所以不建议在类的内部赋予List初始值，而是在实例化后，外部赋值。
            }
        }
        #endregion
    }
}
