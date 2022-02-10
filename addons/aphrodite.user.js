// ==UserScript==
// @id          aphrodite
// @name        aphrodite download buttons
// @description e621 aphrodite userscript
// @namespace   murrty.aphrodite
// @version     2.0
// @homepage    https://github.com/murrty/aphrodite
// @updateURL   https://github.com/murrty/aphrodite/raw/master/addons/aphrodite.user.js
// @downloadURL https://github.com/murrty/aphrodite/raw/master/addons/aphrodite.user.js
// @run-at      document-end
// @grant       none
// @include     http*://e621.net/pools/*
// @include     http*://e621.net/posts*
// @include     http*://www.e621.net/pools/*
// @include     http*://www.e621.net/posts*
// ==/UserScript==

function ShowingPost() {
    return (/^http(s)?:\/\/(www.)?e621.net\/posts\/[0-9]+/.test(document.URL));
}

function ShowingPage() {
    return document.URL.indexOf("e621.net/posts") > -1;
}

function ShowingPool() {
    return (/^http(s)?:\/\/(www.)?e621.net\/pools\/[0-9]+/.test(document.URL));
}

function PostIsPool() {
    return document.getElementsByClassName('pool-nav').length > 0;
}

function AddPostButtons(IncludePoolButtons) {
    var imageDiv = document.createElement('div');
    var imageLink = document.createElement('a');
    var imageSettings = document.createElement('a');
    var fImagesSidebar = document.getElementById('blacklist-box');

    imageLink.id = "download-image";
    imageLink.title = "Download this image with aphrodite";
    imageLink.href = "aphrodite:image \"" + document.URL + "\"";
    imageLink.appendChild(document.createTextNode('download image'));
    imageDiv.appendChild(imageLink);
    imageDiv.appendChild(document.createElement('br'));

    if (document.getElementsByName('tags').length > 0) {
        var foundTags = document.getElementsByName('tags')[0].value;
        if (foundTags != "") {
            var tagsDownloadLink = document.createElement('a');
            tagsDownloadLink.id = "download-tags-image";
            tagsDownloadLink.title = "Download the searched tag(s)"
            tagsDownloadLink.href = "aphrodite:tags \"" + foundTags +"\"";
            tagsDownloadLink.appendChild(document.createTextNode('download tag(s)'));
            imageDiv.appendChild(tagsDownloadLink);
            imageDiv.appendChild(document.createElement('br'));
        }
    }

    if (IncludePoolButtons) {
        var PoolNameElement = document.getElementsByClassName("pool-name");
        var PoolLink = PoolNameElement[0].childNodes[1].href;
        if (typeof PoolLink != "undefined") {
            var PoolName = PoolNameElement[0].childNodes[1].innerHTML.substring(6);
            var PoolDownloadButton = document.createElement('a');
            var PoolWishlistButton = document.createElement('a');

            PoolDownloadButton.href = "aphrodite:pool \"" + PoolLink + "\"";
            PoolDownloadButton.id = "download-pool";
            PoolDownloadButton.title = "Download this pool using aphrodite";
            PoolDownloadButton.appendChild(document.createTextNode('download pool'));
            imageDiv.appendChild(PoolDownloadButton);
            imageDiv.appendChild(document.createElement('br'));

            PoolWishlistButton.href = "aphrodite:poolwl \"" + PoolLink + "\" \""  + PoolName.replace(new RegExp("\"", "g"), "\\\"") + "\"";
            PoolWishlistButton.id = "pool-add-to-wishlist";
            PoolWishlistButton.title = "Add pool to aphrodite's wishlist";
            PoolWishlistButton.appendChild(document.createTextNode('add to pool wishlist'));
            imageDiv.appendChild(PoolWishlistButton);
            imageDiv.appendChild(document.createElement('br'));
        }
    }

    imageSettings.id = "aphrodite-settings";
    imageSettings.href = "aphrodite:configuresettings";
    imageSettings.title = "Change the Image Downloader settings";
    imageSettings.appendChild(document.createTextNode('aphrodite settings'));
    imageDiv.appendChild(imageSettings);

    imageDiv.id = "paginator";
    imageDiv.className = "status-notice";
    fImagesSidebar.parentNode.insertBefore(imageDiv, fImagesSidebar);
    fImagesSidebar.parentNode.insertBefore(document.createElement('br'), fImagesSidebar);
}

function AddPageButtons() {
    var tagDiv = document.createElement('div');
    var pageDownloadLink = document.createElement('a');
    var tagSettings = document.createElement('a');
    var tagElement = document.getElementById('blacklist-box');

    pageDownloadLink.id = "download-page";
    pageDownloadLink.href = "aphrodite:page \"" + document.URL + "\"";
    pageDownloadLink.title = "Download this page using aphrodite\n\nWarning: If any new images are uploaded before the API gets parsed, the images located on this page may differ from the ones that are downloaded..";
    pageDownloadLink.appendChild(document.createTextNode('download this page'));
    tagDiv.appendChild(pageDownloadLink);
    tagDiv.appendChild(document.createElement('br'));

    if (document.URL.indexOf("?tags=") > -1 || document.URL.indexOf("&tags=") > -1) {
        if (document.getElementsByName('tags').length > 0) {
            var foundTags = document.getElementsByName('tags')[0].value;
            if (foundTags != "") {
                var tagDownloadTags = document.createElement('a');
                tagDownloadTags.id = "download-tags";
                tagDownloadTags.href = "aphrodite:tags \"" + foundTags +"\"";
                tagDownloadTags.title = "Download searched tag(s) using aphrodite";
                tagDownloadTags.appendChild(document.createTextNode('download tag(s)'));
                tagDiv.appendChild(tagDownloadTags);
                tagDiv.appendChild(document.createElement('br'));
            }
        }
    }

    tagSettings.id = "download-settings";
    tagSettings.href = "aphrodite:configuresettings";
    tagSettings.title = "Change aphrodite's tag downloading settings";
    tagSettings.appendChild(document.createTextNode('aphrodite settings'));
    tagDiv.appendChild(tagSettings);

    tagDiv.id = "paginator";
    tagDiv.style = "display: normal; text-align: left; padding: 2px 2px 1em;"
    tagDiv.className = "pagination";
    tagElement.parentNode.insertBefore(tagDiv, tagElement);
}

function AddPoolButtons() {
    var poolDiv = document.createElement('div');
    var poolLink = document.createElement('a');
    var poolSettings = document.createElement('a');
    var poolAddToWishlist = document.createElement('a');
    var poolShowWishlist = document.createElement('a');
    var fPoolElement = document.getElementById('blacklist-box');

    poolLink.href = "aphrodite:pool \"" + document.URL + "\"";
    poolLink.id = "download-pool";
    poolLink.style = "position: relative;left: 5px";
    poolLink.title = "Download this pool using aphrodite";
    poolLink.appendChild(document.createTextNode('download pool'));
    poolDiv.appendChild(poolLink);

    poolAddToWishlist.href = "aphrodite:poolwl \"" + document.URL + "\" \"" + document.title.substring(7, document.title.length - 7).replace(new RegExp("\"", "g"), "\\\"") + "\"";
    poolAddToWishlist.id = "pool-add-to-wishlist";
    poolAddToWishlist.style = "position: relative;left: 25px";
    poolAddToWishlist.title = "Add pool to aphrodite's wishlist";
    poolAddToWishlist.appendChild(document.createTextNode('add to wishlist'));
    poolDiv.appendChild(poolAddToWishlist);

    poolShowWishlist.href = "aphrodite:poolwl";
    poolShowWishlist.id = "pool-show-wishlist";
    poolShowWishlist.style = "position: relative;left: 45px";
    poolShowWishlist.title = "Show the pools in the wishlist";
    poolShowWishlist.appendChild(document.createTextNode('show wishlist'));
    poolDiv.appendChild(poolShowWishlist);

    poolSettings.href = "aphrodite:pools configuresettings";
    poolSettings.id = "pool-settings";
    poolSettings.style = "position: relative;left: 65px";
    poolSettings.title = "Change aphrodite's pool downloading settings";
    poolSettings.appendChild(document.createTextNode('pool settings'));
    poolDiv.appendChild(poolSettings);

    poolDiv.id = "paginator";
    poolDiv.style = "display: normal; text-align: left; padding: 0px 0px 0px;"
    poolDiv.className = "pageination";
    fPoolElement.parentNode.insertBefore(poolDiv, fPoolElement);
    fPoolElement.parentNode.insertBefore(document.createElement('br'), fPoolElement);
}

if (ShowingPost()) {
    AddPostButtons(PostIsPool());
}
else if (ShowingPool()) {
    AddPoolButtons();
}
else if (ShowingPage()) {
    AddPageButtons();
}