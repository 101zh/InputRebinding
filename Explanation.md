# Explanation

This is where some code is explained and where changes are suggested

## Changes That You May Want to Make

### Scripts

Make sure to ...

- Refactor the class name for your generated C# class in `InputManager.cs` on line 10.
- Make sure to assign the correct scope for rebinding in `InputManagger.cs` on line 34.

### In Editor

Make sure to ...

- Assign serialized fields.
- Put `Binding.prefab` and `Header.prefab` under `Resources/Prefabs`.
- Make EventSystem object with a the new input system version of it.
- Have the following in your scene: `Settings.prefab` dragged in from assets and an InputManager.

### Navigation

At the moment, the scrollrect for bindings doesn't scroll when using the keyboard to navigate.\
Feature is coming soon! Otherwise, you can customize that to your need.

### Appearance

I would obviously suggest that you change the UI to match your game.\
Right now, the style is unity's default. If I were you, I would make some changes.

## Explanation of Structure

The `InputManager` will be attached to an object that will persist through every scene.
This means that you can statically access most methods and fields.\
To avoid excessive object allocation, I would suggest that `InputAction`s should be accessed from the `InputManager` just like in `TitleMenuManager.cs`.
