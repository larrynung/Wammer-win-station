﻿#region

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

#endregion

namespace Waveface.API.V2
{
    #region Entities

    public class Post
    {
        public string post_id { get; set; }
        public string creator_id { get; set; }
        public string code_name { get; set; }
        public string device_id { get; set; }
        public string group_id { get; set; }
        public string timestamp { get; set; }
        public string content { get; set; }
        public string type { get; set; }
        public string status { get; set; }

        public int comment_count { get; set; }
        public List<Comment> comments { get; set; }

        public int attachments_count { get; set; }
        public List<Attachment> attachments { get; set; }
        public List<string> attachment_id_array { get; set; }

        public Preview_OpenGraph preview { get; set; }
        public string soul { get; set; }
    }

    public class Attachment
    {
        public string group_id { get; set; }
        public string description { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string file_name { get; set; }
        public string image { get; set; }
        public string object_id { get; set; }
        public string creator_id { get; set; }
        public string modify_time { get; set; }
        public string code_name { get; set; }
        public string file_size { get; set; }
        public string type { get; set; }
        public string device_id { get; set; }
        public string mime_type { get; set; }
        public string md5 { get; set; }

        public ImageMeta image_meta{ get; set; }
        //@ public string doc_meta{ get; set; }
    }

    #region 

    public class ImageMeta
    {
        public ImageMetaItem small { get; set; }
        public ImageMetaItem medium { get; set; }
        public ImageMetaItem large { get; set; }
        public ImageMetaItem square { get; set; }  
    }

    public class ImageMetaItem
    {
        public string url { get; set; }
        public string file_name { get; set; }       
        public string height { get; set; }
        public string width { get; set; }  
        public string modify_time { get; set; }
        public string file_size { get; set; }       
        public string mime_type { get; set; }
        public string md5 { get; set; }  
    }

    #endregion

    public class Comment
    {
        public string comment_id { get; set; }
        public string creator_id { get; set; }
        public string post_id { get; set; }
        public string code_name { get; set; }
        public string timestamp { get; set; }
        public string content { get; set; }
    }

    public class Device
    {
        public string device_name { get; set; }
        public string device_id { get; set; }
    }

    public class User
    {
        public string user_id { get; set; }
        public string email { get; set; }
        public string nickname { get; set; }
        public string avatar_url { get; set; }
        public List<Device> devices { get; set; }
    }

    public class Group
    {
        public string group_id { get; set; }
        public string creator_id { get; set; }
        public string station_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class DiskUsage
    {
        public string avail { get; set; }
        public string group_id { get; set; }
        public string used { get; set; }
    }

    public class Station
    {
        public string station_id { get; set; }
        public string creator_id { get; set; }
        public string timestamp { get; set; }
        public string last_seen { get; set; }
        public string location { get; set; }
        public List<DiskUsage> diskusage { get; set; }
        public string status { get; set; }
    }

    public class Preview_OpenGraph
    {
        public string provider_url { get; set; }
        public string provider_name { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        public string thumbnail_url { get; set; }
        public string type { get; set; }
        public string thumbnail_width { get; set; }
        public string thumbnail_height { get; set; }
    }

    public class Preview_AdvancedOpenGraph
    {
        public string provider_url { get; set; }
        //public OGS_Object @object { get; set; }
        public string description { get; set; }
        public string original_url { get; set; }
        public string url { get; set; }
        public List<OGS_Image> images { get; set; }
        public bool safe { get; set; }
        public string provider_display { get; set; }
        public string author_name { get; set; }
        public string content { get; set; }
        public string favicon_url { get; set; }
        //public List<object> place { get; set; }
        public string author_url { get; set; }
        //public List<object> embeds { get; set; }
        public string title { get; set; }
        public string provider_name { get; set; }
        public int cache_age { get; set; }
        public string type { get; set; }
        //public List<object> @event { get; set; }
    }

    public class OGS_Image
    {
        public string url { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }

    public class Object_File
    {
        public string file_type { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string mime_type { get; set; }
        public string file_size { get; set; }
    }

    public class Image_Meta
    {
    }

    public class SearchFilter
    {
        public string timestamp { get; set; }
        public string creator_id { get; set; }
        public string tag { get; set; }
        public string searchfilter_id { get; set; }
        public string filter_name { get; set; }
        public JObject filter { get; set; }
    }

    public class Stations
    {
        public string status { get; set; }
        public string timestamp { get; set; }
        public string station_id { get; set; }
        public string creator_id { get; set; }
        public string location { get; set; }
        public string last_seen { get; set; }
    }

    public class Apikey
    {
        public string apikey { get; set; }
        public string name { get; set; }
    }

    #endregion

    public class General_R
    {
        public string status { get; set; }
        public string session_token { get; set; }
        public string session_expires { get; set; }
        public string timestamp { get; set; }
        public string api_ret_code { get; set; }
        public string api_ret_message { get; set; }
    }

    #region MR_auth

    public class MR_auth_signup : General_R
    {
        public User user { get; set; }
    }

    public class MR_auth_login : General_R
    {
        public User user { get; set; }
        public Device device { get; set; }
        public List<Group> groups { get; set; }
        public List<Station> stations { get; set; }
        public Apikey apikey { get; set; }
    }

    public class MR_auth_logout
    {
        public string status { get; set; }
        public string timestamp { get; set; }
        public string api_ret_code { get; set; }
    }

    #endregion

    #region MR_user

    public class MR_users_get : General_R
    {
        public User user { get; set; }
        public List<Group> groups { get; set; }
    }

    public class MR_users_update : General_R
    {
        public User user { get; set; }
    }

    public class MR_users_passwd : General_R
    {
    }

    public class MR_users_findMyStation : General_R
    {
        public List<Group> groups { get; set; }
        public List<Station> stations { get; set; }
    }

    #endregion

    #region MR_groups

    public class MR_groups_create : General_R
    {
        public Group group { get; set; }
    }

    public class MR_groups_get : General_R
    {
        public Group group { get; set; }
        public List<User> active_members { get; set; }
    }

    public class MR_groups_update : General_R
    {
        public Group group { get; set; }
    }

    public class MR_groups_delete : General_R
    {
    }

    public class MR_groups_inviteUser : General_R
    {
    }

    public class MR_groups_kickUser : General_R
    {
    }

    #endregion

    #region MR_posts

    public class MR_posts_getSingle : General_R
    {
        public string group_id { get; set; }

        public Post post { get; set; }
    }

    public class MR_posts_get : General_R
    {
        public string group_id { get; set; }

        public int get_count { get; set; }
        public int remaining_count { get; set; }
        public int newer_count { get; set; }

        public List<Post> posts { get; set; }
    }

    public class MR_posts_getLatest : General_R
    {
        public string group_id { get; set; }

        public int get_count { get; set; }
        public int total_count { get; set; }

        public List<Post> posts { get; set; }
        public List<User> users { get; set; }
    }

    public class MR_posts_new : General_R
    {
        public string group_id { get; set; }

        public Post post { get; set; }
    }

    #endregion

    #region MR_comments

    public class MR_posts_newComment : General_R
    {
        public string group_id { get; set; }
        public string post_id { get; set; }

        public int comment_count { get; set; }
        public string comment_id { get; set; }
        public List<Comment> comments { get; set; }
    }

    public class MR_posts_getComments : General_R
    {
        public string group_id { get; set; }
        public string post_id { get; set; }

        public int comment_count { get; set; }
        public List<Comment> comments { get; set; }
    }

    #endregion

    #region MR_previews

    public class MR_previews_get : General_R
    {
        public Preview_OpenGraph preview { get; set; }
    }

    public class MR_previews_get_adv : General_R
    {
        public Preview_AdvancedOpenGraph preview { get; set; }
    }

    #endregion

    #region MR_attachments

    public class MR_attachments_upload : General_R
    {
        public string object_id { get; set; }
    }

    public class MR_attachments_get : General_R
    {
        public string description { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string image { get; set; }
        public string modify_time { get; set; }
        public string object_id { get; set; }
        public string device_name { get; set; }
        public string creator_id { get; set; }

        public Image_Meta image_meta { get; set; }

        public string file_size { get; set; }
        public string type { get; set; }
        public string mime_type { get; set; }
    }

    public class MR_attachments_delete : General_R
    {
    }

    #endregion

    #region MR_searchfilters

    public class MR_searchfilters_item : General_R
    {
        public SearchFilter searchfilter { get; set; }
    }

    public class MR_searchfilters_list : General_R
    {
        public List<SearchFilter> search_filters { get; set; }
        public int search_filter_count { get; set; }
    }

    #endregion

    #region MR_hide

    public class MR_hide_ret : General_R
    {
        public string object_type { get; set; }
        public string object_id { get; set; }
        public string object_status { get; set; }
    }

    public class MR_hide_list : General_R
    {
        public int post_count { get; set; }
        public List<Post> posts { get; set; }
        public int attachment_count { get; set; }
        public List<Attachment> attachments { get; set; }
    }

    #endregion
}