local utils = require("utils")

local modName = "DavsRandomStuff"
local springType =  modName .. "/DangerDashSpring"


local springOpts = { "Up", "Left", "Down", "Right" }

local function getOrientation(entity, offset)
  local index = 1
  for i,v in ipairs(springOpts) do
    if (springType .. v) == entity._name then
      index = i
      break
    end
  end
  if offset == nil then
    return index
  end
  return springOpts[(index + offset - 1) % 4 + 1]
end

local function rotate(room, entity, direction)
    local dir = getOrientation(entity)
    local swap = entity.width
    entity.height = entity.width
    entity.width = swap
    entity._name = getOrientation(entity, direction)
    return true
end

local function flip(room, entity, horizontal, vertical)
  if getOrientation(entity) % 2 == 0 then
    if horizontal then
      entity._name = getOrientation(entity, 2)
    end
    return horizontal
  end

  if vertical then
    entity._name = getOrientation(entity, 2)
  end
  return vertical
end

local function getPlacement(name, rotation, xOff, yOff, x, y)
  local spring = {}

  local spritePath = "objects/" .. springType .."/"

  spring.name = springType .. name
  spring.depth = -8501
  spring.justification = {0.5, 1.0}
  spring.texture = spritePath .. "00"
  spring.rotation = rotation
  spring.flip = flip
  spring.rotate = rotate
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