namespace ClinicalTools.SimEncounters
{
    public class XmlCollectionInfo
    {
        public XmlNodeInfo CollectionNode { get; }
        public XmlNodeInfo ElementNode { get; }

        public XmlCollectionInfo(string collectionName, string elementName)
        {
            CollectionNode = new XmlNodeInfo(collectionName);
            ElementNode = new XmlNodeInfo(elementName);
        }
        public XmlCollectionInfo(XmlNodeInfo collectionNode, XmlNodeInfo elementNode)
        {
            CollectionNode = collectionNode;
            ElementNode = elementNode;
        }
    }
}