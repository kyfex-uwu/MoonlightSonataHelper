local utils = require("utils")

local rain = {
    name="MoonlightSonataHelper/Rain",
    canResize = {true, false},
    fieldInformation = {
        
    },
    placements = {
        name="default",
        data = {
            width = 8
        }
    }
}

function rain.draw(room, entity, viewport)
    local pr, pg, pb, pa = love.graphics.getColor()
    
    love.graphics.setColor(43/255, 144/255, 227/255, 0.3)
    local width=entity.width or 0
    local height = room.height
    love.graphics.polygon("fill", {
        entity.x, entity.y,
        entity.x+width, entity.y,
        entity.x+--[[entity.skew*height+]]width, height,
        entity.x+--[[entity.skew*height]]0, height
    })

    love.graphics.setColor(pr, pg, pb, pa)
end

function rain.depth(room, entity)
    return -8500  --todo?
end

function rain.selection(room, entity)
    local skewW=--[[entity.skew*entity.height]]0
    return utils.rectangle(math.min(entity.x,entity.x+skewW), entity.y, 
        entity.width+math.abs(skewW), room.height-entity.y)
end

return rain