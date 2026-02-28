local utils = require("utils")

local modName = "DavsRandomStuff"
local springType =  modName .. "/DangerDashSpring"

local function getPlacement(name, rotation, xOff, yOff, x, y)
  local spring = {}

  local spritePath = "objects/" .. springType .."/"

  spring.name = springType .. name
  spring.depth = -8501
  spring.justification = {0.5, 1.0}
  spring.texture = spritePath .. "00"
  spring.rotation = rotation
  spring.selection = function (room, entity)
    return utils.rectangle(entity.x - xOff, entity.y - yOff, x, y)
  end
  spring.placements = {
    name = string.lower(name),
    data = {
      spritePath = spritePath,
      spikesPath = spritePath,
      playerCanUse = true,
      ignoreHoldables = false,
      ignoreRedBoosters = false,
      spikesOutline = false
    }
  }
  return spring
end

local springUp = getPlacement("Up", nil, 6, 5, 12, 5)

local springDown = getPlacement("Down", math.pi, 6, 0, 12, 5)

local springLeft = getPlacement("Left", math.pi / 2, 0, 6, 5, 12)

local springRight = getPlacement("Right", -math.pi / 2, 5, 6, 5, 12)

return {
  springUp,
  springDown,
  springRight,
  springLeft
}