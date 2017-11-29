## Description
DontWasteWeight is a C# console application that applied an A* search algorithm for finding the most efficient way of loading a barbell for a given workout.  Given a supply of plates of varying weight, DontWasteWeight will use A* to find the best way to load and unload the weights for the fewest number of plates moved.

## Terms:

- LiftSession - implements IBestFirstSearchable. 1 LiftSession is 1 node, which gets expanded (next possible nodes found) for finding solution

- LiftSets (Stack of LiftSet)

- LiftSet - Represents a single valid lifting state. i.e. 45# bar with 45s on each side (135# total). Every time we add or remove weights, we create a new LiftSet

- Bar - Keeps track of the weight of the bar, and the current total weight being lifted (Bar weight + Loaded PlateSet(s) weight)

- LoadedPlates (Stack of PlateSets) PlateSets currently loaded on bar (not on ground next to)

- PlateSet - Always consists of 2 Plates. Only PlateSets are added or removed (i.e. a single plate can't be added). ID'd by their weight 45 = total weight of 90

- Plate - Singular plate that only has a weight associated to it. 2, and only 2, of these comprise a PlateSet

- SessionWeightStack: A stack of weights that the "gym" has available for use. We pop from this when we put weights (PlateSets) on the bar

- PulledWeightStack: The stack of weights that represents what we have pulled from the gym's supply to our area.