# Using arguments for aphrodite
Arguments are used the same was as other programs. The protocol also uses these arguments, but more refined and specific.

When dealing with arguments that contain spaces in them (such as multiple tags, or a pool name), you can type them in "quotes", so the system will put them in as one argument.

**The following arguments are supported:**

* "t", "-t", "tag", "-tag", "tags", "-tags" <tags>
	* This downloads tags from e621. If you use this argument with "configuresettings", it will open the tags tab on the settings form.
	* (If the identifier argument is the only one passed, the main form will show with the tags tab activated.)

* "pg", "-pg", "page", "-page" <page>
	* This downloads a specified page from e621. It takes into account page, tags, and image limit. If you use this argument with "configuresettings", it will open the tags tab on the settings form.
	* (If the identifier argument is the only one passed, the main form will show with the pool tab activated.)

* "p", "-p", "pool", "-pool", "pools", "-pools" <pool url/id>
	* This downloads a pool from e621. If you use this argument with "configuresettings", it will open the pool tab on the settings form.
	* (If the identifier argument is the only one passed, the main form will show with the images tab activated.)

* "i", "-i", "image", "-image", "images", "-images" <image url/id>
	* This downloads an image from e621. If you use this argument with "configuresettings", it will open the images tab on the settings form.
	* (If the identifier argument is the only one passed, the main form will show with the images tab activated.)

* "wl", "-wl", "pwl", "-pwl", "poolwl", "-poolwl", "poolwishlist", "-poolwishlist" <pool url (in quotes)> [pool name (in quotes)]
	* This adds a pool to the wishlist. The url is required, the name is not, but you will be asked to input one.
	* (If the identifier argument is the only one passed, the pool wishlist form will show.)

* "rd", "-rd", "redownload", "-redownload", "redownloader", "-redownloader"
	* This opens the redownloader.

* "b", "g", "-b", "-g", "graylist", "blacklist", "-graylist", "-blacklist"
	* This opens the blacklist.

* "c", "s", "-c", "config", "-config", "settings", "-settings", "configuration", "-configuration"
	* This opens the settings. This can also accept sub-arguments for what tab to activate:
	* "t", "-t", "tag", "-tag", "tags", "-tags"
	* "p", "-p", "pool", "-pool", "pools", "-pools"
	* "i", "-i", "image", "-image", "images", "-images"
	* "m", "-m", "misc", "-misc"
	* "pr", "-pr", "protocol", "-protocol"
	* "s", "-s", "schema", "-schema", "schemas", "-schemas"
	* "po", "-po", "ini", "-ini", "portable", "-portable"

* "up", "-up", "updateprotocol", "-updateprotocol", "installprotocol", "-installprotocol"
	* This will update or install the protocol to the computer.

* "pr", "-pr", "protocol", "-protocol"
	* This will open the settings and activate the protocol tab.

* "portable", "-portable"
	* Same as above, but will activate the portable tab.

* "schema", "-schema"
	* Same as above, but will activate the file name schemas tab.

* "fb", "-fb", "furrybooru", "-furrybooru" [tags] (Not yet implemented)
	* This downloads tags from furry booru. If you use this argument with "configuresettings", it will open the tags tab on the settings form.
	* (If the identifier argument is the only one passed, the furrybooru form will show.)

* "ib", "-ib", "inkbunny", "-inkbunny" [keywords] [artist username (artist's gallery)] [user id (users' favorites)]
	* This will download keywords from InkBunny. The other two arguments are optional, but allow more granular control of the search terms. If you use this argument with "configuresettings", it will open the tags tab on the settings form.
	* (If the identifier argument is the only one passed, the inkbunny form will show.)

* "ig", "-ig", "imgur", "-imgur" <album>
	* This will download an album from imgur. If you use this argument with "configuresettings", it will open the tags tab on the settings form.
	* (If the identifier argument is the only one passed, the inkbunny form will show.)