// ==UserScript==
// @id          aphrodite
// @name        aphrodite
// @description e621 general downloader (pools & images based on tags)
// @namespace   murrty.aphrodite
// @version     1.7

// @homepage    https://github.com/murrty/aphrodite
// @updateURL   https://github.com/murrty/aphrodite/raw/master/addons/aphrodite.user.js
// @downloadURL https://github.com/murrty/aphrodite/raw/master/addons/aphrodite.user.js

// @run-at      document-end
// @grant       none

// @include     http*://e621.net/pools/*
// @include     http*://e621.net/posts*
// ==/UserScript==

// Pool
if (document.URL.indexOf("e621.net/posts/") > -1 || document.URL.indexOf("e621.net/pools/") > -1) {
    var poolDiv = document.createElement('div');
    var poolLink = document.createElement('a');
    var poolSettings = document.createElement('a');
    var poolAddToWishlist = document.createElement('a');
    var poolShowWishlist = document.createElement('a');
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
    poolID = tag.id.replace(new RegExp("pool", "g"), '');

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

    poolAddToWishlist.id = "pool-add-to-wishlist";
    poolAddToWishlist.title = "Add pool to aphrodite's wishlist";
    poolAddToWishlist.style = "position: relative;left: 45px";
    poolAddToWishlist.appendChild(document.createTextNode('add to wishlist'));
    poolDiv.appendChild(poolAddToWishlist);

    poolShowWishlist.id = "pool-show-wishlist";
    poolShowWishlist.href = "poolwl:showwl";
    poolShowWishlist.title = "Show the pools in the wishlist";
    poolShowWishlist.style = "position: relative;left: 65px";
    poolShowWishlist.appendChild(document.createTextNode('show wishlist'));
    poolDiv.appendChild(poolShowWishlist);

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
        poolAddToWishlist.href = "poolwl:" + document.URL + "|" + document.title.substring(7, document.title.length - 7).replace(new RegExp(" ", "g"), '%$|%');
        fPoolElement.parentNode.insertBefore(poolDiv, fPoolElement);
        fPoolElement.parentNode.insertBefore(poolSpacer, fPoolElement);
    }
}
// Tag
else if (document.URL.indexOf("e621.net/posts") > -1 || document.URL.indexOf("e621.net/posts?") > -1) {
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
    tagDownloadLink.title = "Download this page using aphrodite\r\n\r\nWarning: New images that get uploaded as the API is parsed will overwite the last images.";
    tagDownloadLink.appendChild(document.createTextNode('download this page'));
    tagDiv.appendChild(tagDownloadLink);
    tagDiv.appendChild(tagSpacer);

    if (document.URL.indexOf("?tags=") > -1 || document.URL.indexOf("&tags=") > -1) {
        tagDownloadTags.id = "download-tags";
        var foundTags = document.getElementsByName('tags')[0].value;
        tagDownloadTags.href = "tags:" + foundTags.replace(new RegExp(" ", "g"), '|');
        tagDownloadTags.title = "Download searched tag(s) using aphrodite";
        tagDownloadTags.appendChild(document.createTextNode('download searched tag(s)'));
        tagDiv.appendChild(tagDownloadTags);
        tagDiv.appendChild(tagSpacerTwo);
    }

    tagSettings.id = "download-settings";
    tagSettings.href = "tags:configuresettings";
    tagSettings.title = "Change aphrodite's tag downloading settings";
    tagSettings.appendChild(document.createTextNode('change downloader settings'));
    tagDiv.appendChild(tagSettings);
    tagElement.parentNode.insertBefore(tagDiv, tagElement);
}