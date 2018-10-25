using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Uncas.Web
{
    /// <summary>
    ///     Represents a web feed
    /// </summary>
    public class Feed
    {
        #region Private properties

        /// <summary>
        ///     The total number of enclosures in the feed items of this feed
        /// </summary>
        /// <remarks>
        ///     Only when this number is larger than 0, the RSS feed displays iTunes info.
        /// </remarks>
        private int NumberOfEnclosures
        {
            get
            {
                var numberOfEnclosures = 0;
                foreach (var item in Items)
                    if (item.Enclosure != null)
                        numberOfEnclosures++;
                return numberOfEnclosures;
            }
        }

        #endregion

        #region Public static methods

        /// <summary>
        ///     Common method that handles a required string parameters
        /// </summary>
        /// <param name="value">The string parameter</param>
        /// <param name="nameOfValue">The name of the parameter</param>
        public static void CheckRequiredValue(string parameter, string nameOfParameter)
        {
            if (string.IsNullOrEmpty(parameter))
                throw new ArgumentNullException(nameOfParameter,
                    string.Format("It is required that {0} has a value", nameOfParameter));
        }

        #endregion

        #region Constructor

        // TODO: consider another constructor with the fields required for an iTunes podcast

        /// <summary>
        ///     Creates a feed with the provided title and description. Valid RSS.
        /// </summary>
        /// <param name="title">The title of the feed</param>
        /// <param name="description">The description of the feed</param>
        public Feed(string title, string description)
        {
            CheckRequiredValue(title, "title");
            CheckRequiredValue(description, "description");
            // Sets the properties:
            Title = title;
            Description = description;
            // Initializes the list of items:
            Items = new List<FeedItem>();
        }

        /// <summary>
        ///     Creates a feed with the provided title, link, description, and author. Valid RSS and Atom.
        /// </summary>
        /// <param name="title">The title of the feed</param>
        /// <param name="description">The description of the feed</param>
        /// <param name="author">The author of the feed</param>
        public Feed(string title, string description, string author)
            : this(title, description)
        {
            CheckRequiredValue(author, "author");
            Author = author;
        }

        #endregion

        #region Public properties

        /// <summary>
        ///     The title of the feed
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The description of the feed
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The author of the feed
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Alternate url, e.g. of a blog web page with the same content
        /// </summary>
        public string AlternateLink { get; set; }

        /// <summary>
        ///     Copyright information
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        ///     The language of the feed
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        ///     Email of the managing editor
        /// </summary>
        public FeedEmailAddress ManagingEditor { get; set; }

        /// <summary>
        ///     Email of the webmaster
        /// </summary>
        public FeedEmailAddress WebMaster { get; set; }

        /// <summary>
        ///     Categories
        /// </summary>
        public string[] Categories { get; set; }

        /// <summary>
        ///     Title of a header image
        /// </summary>
        public string ImageTitle { get; set; }

        /// <summary>
        ///     Url to header image
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        ///     The list of items for the feed
        /// </summary>
        public List<FeedItem> Items { get; set; }

        #endregion Public properties

        #region Public methods

        /// <summary>
        ///     Writes a RSS feed to the HttpResponse of the HttpContext
        /// </summary>
        /// <param name="path"></param>
        public void WriteRSS(string path)
        {
            var rss = WriteRSS();
            File.WriteAllText(path, rss);
        }

        public string WriteRSS()
        {
            using (var memoryStream = new MemoryStream())
            {
                WriteRSS(memoryStream);

                return Encoding.UTF8.GetString(memoryStream.ToArray());
            }
        }

        public void WriteRSS(Stream rs)
        {
            #region Creates the document

            // Sets the content-type:

            // Creates an XmlTextWriter to write the XML data to a string:
            using (var writer = new XmlTextWriter(rs, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                // Writes the document:
                writer.WriteStartDocument();

                writer.WriteStartElement("rss");

                if (NumberOfEnclosures > 0)
                {
                    writer.WriteAttributeString("xmlns:itunes",
                        "http://www.itunes.com/dtds/podcast-1.0.dtd");
                    writer.WriteAttributeString("xmlns:media",
                        "http://search.yahoo.com/mrss/");
                    writer.WriteAttributeString("xml:lang", Language);
                }

                writer.WriteAttributeString("version", "2.0");

                // Creates the channel:
                writer.WriteStartElement("channel");

                #endregion

                #region Feed channel info

                #region Required fields

                writer.WriteElementString("title", Title);
                writer.WriteElementString("link", Link);
                writer.WriteElementString("description", Description);

                #region atom:link

                writer.WriteStartElement("atom:link");
                writer.WriteAttributeString("xmlns:atom", "http://www.w3.org/2005/Atom");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("href", Link);
                writer.WriteAttributeString("type", "application/rss+xml");
                writer.WriteEndElement();

                #endregion atom:link

                #endregion Required fields

                #region Fields with fixed values (fixed by Ole L. Sørensen)

                writer.WriteElementString("ttl", "60");
                writer.WriteElementString("docs"
                    , "http://cyber.law.harvard.edu/rss/rss.html");

                var now = DateTime.UtcNow;
                writer.WriteElementString("pubDate", now.ToString("r"));
                writer.WriteElementString("lastBuildDate", now.ToString("r"));

                #endregion Fields with fixed values

                #region Optional fields

                if (Copyright != null)
                    writer.WriteElementString("copyright", Copyright);
                if (Language != null)
                    writer.WriteElementString("language", Language);
                if (ManagingEditor != null)
                    writer.WriteElementString("managingEditor", ManagingEditor.ToString());
                if (WebMaster != null)
                    writer.WriteElementString("webMaster", WebMaster.ToString());

                // TODO: include rating info?
                // TODO: include cloud info?
                if (Categories != null)
                    foreach (var category in Categories)
                        writer.WriteElementString("category", category);

                #region iTunes info

                if (NumberOfEnclosures > 0)
                {
                    // TODO: consider itunes:explicit, maybe via an enumeration...
                    // Possible values for itunes:explicit: yes, clean, no
                    // HACK: I use itunes:explicit="no"
                    writer.WriteElementString("itunes:explicit", "no");

                    var description = GetDescription();

                    writer.WriteElementString("itunes:subtitle", description);

                    writer.WriteElementString("itunes:summary", Description);
                    if (Author != null)
                        writer.WriteElementString("itunes:author", Author);

                    // TODO: does itunes:owner equal ManagingEditor?
                    if (ManagingEditor != null)
                    {
                        writer.WriteStartElement("itunes:owner");
                        writer.WriteElementString("itunes:name", ManagingEditor.RealName);
                        writer.WriteElementString("itunes:email", ManagingEditor.EmailAddress);
                        writer.WriteEndElement();
                    }

                    if (ImageUrl != null)
                    {
                        writer.WriteStartElement("itunes:image");
                        writer.WriteAttributeString("href", ImageUrl);
                        writer.WriteEndElement();
                    }

                    if (Categories != null)
                        foreach (var category in Categories)
                        {
                            writer.WriteStartElement("itunes:category");
                            writer.WriteAttributeString("text", category);
                            writer.WriteEndElement();
                        }
                }

                #endregion iTunes info

                #region Image info

                if (ImageUrl != null)
                {
                    writer.WriteStartElement("image");
                    writer.WriteElementString("title", ImageTitle);
                    writer.WriteElementString("url", ImageUrl);
                    writer.WriteElementString("link", Link);
                    writer.WriteEndElement();
                }

                #endregion Image info

                #endregion Optional fields

                #endregion Feed channel info

                #region Feed items

                foreach (var item in Items)
                {
                    writer.WriteStartElement("item");

                    writer.WriteElementString("title", item.Title);

                    #region Optional fields

                    if (item.Link != null)
                    {
                        writer.WriteElementString("link", item.Link);
                        // TODO: use something else for item guid???
                        writer.WriteElementString("guid", item.Link);
                    }

                    if (item.Description != null)
                        writer.WriteElementString("description", item.Description);
                    if (item.Author != null)
                        writer.WriteElementString("author", item.Author.ToString());
                    if (item.Category != null)
                        writer.WriteElementString("category", item.Category);
                    if (item.PublishDate.HasValue)
                        writer.WriteElementString("pubDate", item.PublishDate.Value.ToString("r"));

                    #region Enclosure info

                    if (item.Enclosure != null)
                    {
                        // e.g. pod cast:
                        writer.WriteStartElement("enclosure");
                        writer.WriteAttributeString("url", item.Enclosure.Url);
                        writer.WriteAttributeString("type", item.Enclosure.MimeType);
                        writer.WriteAttributeString("length", item.Enclosure.Length.ToString());
                        writer.WriteEndElement();

                        #region iTunes info

                        if (item.Description != null)
                            writer.WriteElementString("itunes:subtitle", item.Description);
                        if (item.Author != null)
                            writer.WriteElementString("itunes:author", item.Author.EmailAddress);
                        if (item.Description != null)
                            writer.WriteElementString("itunes:summary", item.Description);

                        if (item.ImageUrl != null)
                        {
                            writer.WriteStartElement("itunes:image");
                            writer.WriteAttributeString("href", item.ImageUrl);
                            writer.WriteEndElement();
                        }

                        writer.WriteStartElement("media:content");
                        writer.WriteAttributeString("url", item.Enclosure.Url);
                        writer.WriteAttributeString("fileSize"
                            , item.Enclosure.Length.ToString());
                        writer.WriteAttributeString("type", item.Enclosure.MimeType);
                        writer.WriteEndElement();

                        #endregion iTunes info
                    }

                    #endregion Enclosure info

                    #endregion Optional fields

                    writer.WriteEndElement();
                }

                #endregion Feed items

                #region Finishes the document

                writer.WriteEndElement();
                writer.WriteEndElement();

                writer.Close();
            }

            #endregion
        }

        private string GetDescription()
        {
            var description = "";
            if (!string.IsNullOrEmpty(Description))
            {
                var descriptionLength = Description.Length;

                if (descriptionLength > 255)
                    description = Description.Substring(0, 255);
                else
                    description = Description;
            }

            return description;
        }

        public string Link { get; set; }

        /// <summary>
        ///     Writes an Atom feed to the HttpResponse of the HttpContext
        /// </summary>
        /// <param name="context">The HttpContext</param>
        public void WriteAtom(string path)
        {
            #region Creates the xml document

            // Set the content-type

            // Use an XmlTextWriter to write the XML data to a string...
            using (var writer = new XmlTextWriter
                (path, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;

                // document info
                writer.WriteStartDocument();
                writer.WriteStartElement("feed");
                writer.WriteAttributeString("xmlns", "http://www.w3.org/2005/Atom");

                #endregion Creates the xml document

                #region Feed info

                #region Required fields

                writer.WriteStartElement("title");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(Title);
                writer.WriteEndElement();

                writer.WriteStartElement("link");
                writer.WriteAttributeString("rel", "self");
                writer.WriteAttributeString("type", "application/atom+xml");
                writer.WriteAttributeString("href", Link);
                writer.WriteEndElement();

                writer.WriteStartElement("subtitle");
                writer.WriteAttributeString("type", "html");
                writer.WriteString(Description);
                writer.WriteEndElement();

                // TODO: use another id than the link?
                writer.WriteElementString("id", Link);

                #endregion Required fields

                #region Optional fields

                var now = DateTime.UtcNow;
                writer.WriteElementString("updated"
                    , XmlConvert.ToString(now, XmlDateTimeSerializationMode.Utc));

                if (Author != null)
                {
                    writer.WriteStartElement("author");
                    writer.WriteElementString("name", Author);
                    writer.WriteEndElement();
                }

                if (AlternateLink != null)
                {
                    writer.WriteStartElement("link");
                    writer.WriteAttributeString("rel", "alternate");
                    writer.WriteAttributeString("type", "text/html");
                    writer.WriteAttributeString("href", AlternateLink);
                    writer.WriteEndElement();
                }

                #endregion Optional fields

                #endregion Feed info

                #region Writes feed items

                foreach (var item in Items)
                {
                    #region item info

                    writer.WriteStartElement("entry");

                    writer.WriteElementString("title", item.Title);

                    #region Optional fields

                    if (item.Link != null)
                    {
                        writer.WriteStartElement("link");
                        writer.WriteAttributeString("href", item.Link);
                        writer.WriteEndElement();

                        // TODO: should another method for generating a unique item id be used?
                        writer.WriteElementString("id", item.Link);
                    }

                    // TODO: validation warning: "Two entries with the same value for atom:updated"
                    // TODO: do not use datetime.now, but require a publish date?
                    var updated = DateTime.UtcNow;
                    if (item.PublishDate.HasValue)
                        updated = item.PublishDate.Value;
                    writer.WriteElementString("updated"
                        , XmlConvert.ToString(updated
                            , XmlDateTimeSerializationMode.Utc));

                    if (item.Description != null)
                    {
                        writer.WriteStartElement("content");
                        writer.WriteAttributeString("type", "html");
                        writer.WriteString(item.Description);
                        writer.WriteEndElement();
                    }
                    else
                    {
                        // TODO: consider this (required) element, when there is no description...
                        writer.WriteElementString("content", Title);
                    }

                    if (item.Enclosure != null)
                    {
                        writer.WriteStartElement("link");
                        writer.WriteAttributeString("rel", "enclosure");
                        writer.WriteAttributeString("type", item.Enclosure.MimeType);
                        writer.WriteAttributeString("length", item.Enclosure.Length.ToString());
                        writer.WriteAttributeString("href", item.Enclosure.Url);
                        writer.WriteEndElement();
                    }

                    #endregion Optional fields

                    writer.WriteEndElement();

                    #endregion
                }

                #endregion

                #region Finishes the document

                writer.WriteEndElement();
                writer.Close();
            }

            #endregion Finishes the document
        }

        #endregion
    }
}