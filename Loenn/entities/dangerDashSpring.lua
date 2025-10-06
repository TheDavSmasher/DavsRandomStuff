local utils = require("utils")

local function getPlacement(name, rotation, xOff, yOff, x, y)
  local spring = {}

  spring.name = "DavsRandomStuff/DangerDashSpring" .. name
  spring.depth = -8501
  spring.justification = {0.5, 1.0}
  spring.texture = "objects/DavsRandomStuff/DangerDashSpring/00"
  spring.rotation = rotation
  spring.selection = function (room, entity)
    return utils.rectangle(entity.x - xOff, entity.y - yOff, x, y)
  end
  spring.placements = {
    name = string.lower(name),
    data = {
      spritePath = "objects/DavsRandomStuff/DangerDashSpring/",
      playerCanUse = true,
      ignoreHoldables = false,
      ignoreRedBoosters = false
    }
  }
  return spring
end

local springUp = getPlacement("Up", nil, 6, 4, 12, 4)

local springDown = getPlacement("Down", math.pi, 6, 0, 12, 4)

local springLeft = getPlacement("Left", math.pi / 2, 0, 6, 4, 12)

local springRight = getPlacement("Right", -math.pi / 2, 4, 6, 4, 12)

return {
  springUp,
  springDown,
  springRight,
  springLeft
}