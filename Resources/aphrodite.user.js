// ==UserScript==
// @name        Aphrodite
// @namespace   https://github.com/murrty/aphrodite
// @version     1.6
// @description e621 general downloader (pools & images based on tags)
// @run-at      document-end
// @include     http*://e621.net/pools/*
// @include     http*://*.e621.net/pools/*
// @include     http*://e621.net/posts*
// @include     http*://*.e621.net/posts*
// @downloadURL https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js
// @updateURL   https://github.com/murrty/aphrodite/raw/master/Resources/aphrodite.user.js
// @grant       none
// ==/UserScript==


// Pool
if (document.URL.indexOf("e621.net/posts/") > -1 || document.URL.indexOf("e621.net/pools/") > -1) {
    var poolDiv = document.createElement('div');
    var poolLink = document.createElement('a');
    var poolWish = document.createElement('a');
    var poolSettings = document.createElement('a');
    var fPoolElement = document.getElementById('blacklist-box');
    var fPoolSidebar = document.getElementsByClassName('post-sidebar-header')[0];
    var poolSpacer = document.createElement('br');


    var poolID;
    var tags = document.getElementsByTagName('*'), tagsLength = tags.length, matches = [], index, tag;
    for (index = 0; index < tagsLength; index++) {
        tag = tags[index];
        if (tag.id.indexOf("pool") > -1) {
            poolID = tag;
            break;
        }
    }
    poolID = tag.id.replace("pool", "");

    poolDiv.id = "paginator";


    poolLink.id = "download-pool";
    poolLink.title = "Download this pool using aphrodite";
    poolLink.appendChild(document.createTextNode('download pool'));
    poolDiv.appendChild(poolLink);
    poolLink.style = "position: relative;left: 5px";

    poolSettings.id = "pool-settings";
    poolSettings.href = "pools:configuresettings";
    poolSettings.title = "Change aphrodite's pool downloading settings";
    poolSettings.appendChild(document.createTextNode('pool settings'));
    poolSettings.style = "position: relative;left: 25px";
    poolDiv.appendChild(poolSettings);
  
    poolWish.id = "pool-add-to-wishlist";
    poolWish.title = "Add pool to aphrodite's wishlist";
    poolWish.style = "position: relative;left: 45px";
    poolWish.appendChild(document.createTextNode('add to wishlist'));


  
    //if (document.URL.indexOf("e621.net/posts/") > -1) {
    //    poolDiv.style = "display: normal; text-align: left; padding: 1em 23px 1em;"
    //    poolDiv.className = "status-notice";
    //    poolLink.href = "pools:https://e621.net/pool/show/" + poolID;
    //    if (tag.id.lastIndexOf("pool", 0) === 0) {
    //        fPoolSidebar.parentNode.insertBefore(poolDiv, fPoolSidebar);
    //    }
    //}
    if (document.URL.indexOf("e621.net/pools/") > -1) {
        poolDiv.style = "display: normal; text-align: left; padding: 0px 0px 0px;"
        poolDiv.className = "pageination";
        poolLink.href = "pools:" + document.URL;
        poolWish.href = "poolwl:" + document.URL + "$" + document.title.substring(6, document.title.length - 7);
        poolDiv.appendChild(poolWish);
        fPoolElement.parentNode.insertBefore(poolDiv, fPoolElement);
        fPoolElement.parentNode.insertBefore(poolSpacer, fPoolElement);
    }
}
// Tag
else if (document.URL.indexOf("e621.net/posts") > -1 || document.URL.indexOf("e621.net/posts?tags=") > -1) {
    var tagDiv = document.createElement('div');
    var tagDownloadLink = document.createElement('a');
    var tagDownloadTags = document.createElement('a');
    var tagSettings = document.createElement('a');
    var tagElement = document.getElementById('blacklist-box');
    var tagSpacer = document.createElement('br');
    var tagSpacerTwo = document.createElement('br');

    tagDiv.id = "paginator";
    tagDiv.style = "display: normal; text-align: left; padding: 2px 2px 1em;"
    tagDiv.className = "pagination";

    tagDownloadLink.id = "download-page";
    tagDownloadLink.href = "tags:" + document.URL;
    tagDownloadLink.title = "Download this page using aphrodite";
    tagDownloadLink.appendChild(document.createTextNode('download this page'));
    tagDiv.appendChild(tagDownloadLink);
    tagDiv.appendChild(tagSpacer);

    tagDownloadTags.id = "download-tag";
    var foundTags = document.getElementsByName('tags')[0].value;
    tagDownloadTags.href = "tags:" + foundTags.replace(" ", " tags:");
    tagDownloadTags.title = "Download searched tag(s) using aphrodite";
    tagDownloadTags.appendChild(document.createTextNode('download searched tag(s)'));
    tagDiv.appendChild(tagDownloadTags);
    tagDiv.appendChild(tagSpacerTwo);

    tagSettings.id = "download-settings";
    tagSettings.href = "tags:configuresettings";
    tagSettings.title = "Change aphrodite's tag downloading settings";
    tagSettings.appendChild(document.createTextNode('change downloader settings'));
    tagDiv.appendChild(tagSettings);
    tagElement.parentNode.insertBefore(tagDiv, tagElement);
}
