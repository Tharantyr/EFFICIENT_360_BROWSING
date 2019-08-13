# EFFICIENT_360_BROWSING

Please note that this Unity project can only be opened in Unity 2018.1, since due to
budget constraints I had to work with several wonky asset packages that are not
supported by the newer versions of Unity. It requires Android build tools as it is
developed for Android phones using the Google VR SDK.

Please visit this link to understand the context of this Unity project and how it was
used for my master thesis research (contains video):
https://www2.projects.science.uu.nl/cs-gmt/index.php?r=project/view&id=138&title_slug=effectively-browsing-360-degree-videos

Direct link to the scientific paper:
https://dspace.library.uu.nl/bitstream/handle/1874/380854/MSC_Thesis_Peter_de_Keijzer__Master_Thesis.pdf?sequence=2&isAllowed=y

All the relevant scripts are found on the Player game object and its child objects,
but the most important ones are on the BigThumb object (which is also a child object of
Player). ThumbnailScript is the main script controlling everything.
