local utils = require("utils")

local function getPlacement(name, rotation, xOff, yOff, x, y)
  local spring = {}
  local springPath = "DavsRandomStuff/DangerDashSpring"

  spring.name = springPath .. name
  spring.depth = -8501
  spring.justification = {0.5, 1.0}
  spring.texture = "objects/".. springPath .."/00"
  spring.rotation = rotation
  spring.selection = function (room, entity)
    return utils.rectangle(entity.x - xOff, entity.y - yOff, x, y)
  end
  spring.placements = {
    name = string.lower(name),
    data = {
      spritePath = "objects/".. springPath .."/",
      playerCanUse = true,
      ignoreHoldables = false,
      ignoreRedBoosters = false
    }
  }
  return spring
end

local springUp = getPlacement("Up", nil, 8, 4, 15, 4)

local springDown = getPlacement("Down", math.pi, 7, 0, 15, 4)

local springLeft = getPlacement("Left", math.pi / 2, 0, 8, 4, 15)

local springRight = getPlacement("Right", -math.pi / 2, 4, 7, 4, 15)

return {
  springUp,
  springDown,
  springRight,
  springLeft
}