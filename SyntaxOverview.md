* Every statement in MTLang will occupy a `Chunk`.
* Each `Chunk` will correspond to a line of code in `Gdscript`.
* All newlines and whitespace are ignored in MTLang.
* Chronology is determined from the literal call sequence.
# Syntax Overview
## Function Invocation
* `func_name([args])` or `func_name!`
## Variable Declarations (with getters/setters)
* `let variable_name([type = 'Variant']) = value`
## Class Declarations (with parameters)
* `class class_name([extends]) { [...] }`
## Object / Scene instantiation (with parameters)
* `make object_name([args])` -- calls `object_name.new([args]);`.
* `let x(Button) = make Button!;` -- calls `var x:Button = Button.new()`
* `venue packed_scene_var` -- calls `get_tree().change_scene_to_packed(packed_scene_var)`
* `summon packed_scene_var` -- calls `get_tree().current_scene.add_child(packed_scene_var)`
## Flow Control
TODO