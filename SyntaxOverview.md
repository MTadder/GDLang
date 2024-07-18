> Much of this document is still unfinished. Read at your own risk.
* Every statement in MTLang will occupy a `Chunk`.
* Each `Chunk` will correspond to a line of code in `GDScript`.
* All newlines and whitespace are ignored in MTLang.
* Chronology is determined from the literal call sequence.
# Syntax Overview
## Function Invocation
* `func_name([args])` or `func_name!`
## Variable Declarations
* `variable_name<[type = 'Variant']> = value`
* `asdf<int> = getRandInt!`
## Class Declarations (with parameters)
* `class class_name<[extends]> { [...] }`
## Object / Scene instantiation (with parameters)
* `make object_name([args])`
* `x<Button> = make!` compiles into:
    * `var x:Button = Button.new();`
* `venue packed_scene` compiles into:
    * `get_tree().change_scene_to_packed(packed_scene);`
* `summon packed_scene` compiles into:
    * `get_tree().current_scene.add_child(packed_scene);`
## Flow Control
###### TODO