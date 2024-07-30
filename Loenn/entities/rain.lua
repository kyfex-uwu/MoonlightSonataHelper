local utils = require("utils")

local rain = {
    name="MoonlightSonataHelper/Rain",
    canResize = {true, true},
    fieldInformation = {
        skew = {
            default=0
        }
    },
    placements = {
        name="default",
        data = {
            skew=0
        }
    }
}

function rain.draw(room, entity, viewport)
    local pr, pg, pb, pa = love.graphics.getColor()
    
    love.graphics.setColor(43/255, 144/255, 227/255, 0.3)
    local width=entity.width or 0
    local height = entity.height or 0
    love.graphics.polygon("fill", {
        entity.x, entity.y,
        entity.x+width, entity.y,
        entity.x+entity.skew*height+width, entity.y+height,
        entity.x+entity.skew*height, entity.y+height
    })

    love.graphics.setColor(pr, pg, pb, pa)
end

function rain.depth(room, entity)
    return -8500  --todo?
end

function rain.selection(room, entity)
    return utils.rectangle(entity.x, entity.y, entity.w, entity.h) --todo
end

return rain